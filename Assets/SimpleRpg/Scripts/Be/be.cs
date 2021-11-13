using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Specialized;
using System.Text;
using UnityEngine;
using System.Globalization;

public static class Vector3Methods
{
	public static Vector3 Vector3_int (this Vector3 v)
	{
		return new Vector3((int)v.x, (int)v.y, (int)v.z);
	}
	public static Vector3 Random_Vector3 (this Vector3 v)
	{
		var r = new Vector3
		(
			(-v.x).RandomMinMax(v.x)
			, 
			(-v.y).RandomMinMax(v.y)
			, 
			(-v.z).RandomMinMax(v.z)
		)
		;
		return r;
	}
	public static Vector3 Random_Vector3 (this Vector3 v, Vector3 b)
	{
		var r = new Vector3
		(
			(v.x).RandomMinMax(b.x)
			, 
			(v.y).RandomMinMax(b.y)
			, 
			(v.z).RandomMinMax(b.z)
		)
		;
		return r;
	}
	public static Vector3 Random_Vector3_int (this Vector3 v, Vector3 b)
	{
		var r = new Vector3
		(
			((int)v.x).RandomMinMax((int)b.x)
			, 
			((int)v.y).RandomMinMax((int)b.y)
			, 
			((int)v.z).RandomMinMax((int)b.z)
		)
		;
		return r;
	}
}
public static class JsonMethods
{
	public static List<T> JSON_TO_OBJECT_LIST<T>(this string json, params string[] separador)
	{
		var jsonL = new List<string>(json.Split(separador, StringSplitOptions.RemoveEmptyEntries));
		return jsonL.ConvertAll(n => n.TO_OBJECT<T>());
	}
	public static string OBJECT_LIST_TO_JSON<T>(this List<T> L, string separador)
	{
		var r = "";
		L.ForEach(n => r += n.TO_JSON() + separador);
		return r;
	}
}
public static class ListMethods
{
	public static void FOR<T> (this List<T> l, Predicate<T> Requisito, params Action<T>[] Methods)
	{
		foreach (var i in l)
		{
			if(Requisito.Invoke(i))
			{
				foreach (var m in Methods)
				{
					m.Invoke(i);
				}
			}
		}
	}
}
public class GameObjectPersistenceModel
{
	public string GameObjectName;
	public List<MonoPersistenceModel> monoPersistenceModels;
}
[System.Serializable]
public class MonoPersistenceModel
{
	public string Tipo;
	public string MonoJson;
	public void Overwritte (GameObject o)
	{
		o.GetComponent(Tipo).OVERWRITTE(MonoJson);	
	}
}
public static class MonoPersistenceMethods
{

	public static void Save (this List<GameObject> g, string FileName)
	{
		List<GameObjectPersistenceModel> r = new List<GameObjectPersistenceModel>();
		foreach (var o in g)
		{
			var c = new List<MonoBehaviour>(o.GetComponents<MonoBehaviour>());
			var w = c.ConvertAll(n => new MonoPersistenceModel(){ Tipo = n.TYPE(), MonoJson = n.TO_JSON()});
			r.Add(new GameObjectPersistenceModel(){ GameObjectName = o.name, monoPersistenceModels = w});
		}
		FileName.SET_PERSISTENCE(ref r);
	}
	public static void Load (this List<GameObject> g, GameObject prefab, string FileName)
	{
		if(!FileName.EXIST_PERSISTENCE())
		{
			return;
		}

		List<GameObjectPersistenceModel> r = new List<GameObjectPersistenceModel>();
		r = FileName.GET_PERSISTENCE(ref r);
		foreach(var w in r)
		{
			var o = prefab.Instantiate();
			o.SetActive(true);
			o.name = w.GameObjectName;
			w.monoPersistenceModels.ForEach(n => o.GetComponent(n.Tipo).OVERWRITTE(n.MonoJson));
			g.Add(o);
		}
	}
	public static void Save (this GameObject o, string FileName)
	{
		List<MonoPersistenceModel> r = new List<MonoPersistenceModel>();
		var c = new List<MonoBehaviour>(o.GetComponents<MonoBehaviour>());
		r = c.ConvertAll(n => new MonoPersistenceModel(){ Tipo = n.TYPE(), MonoJson = n.TO_JSON()});
		FileName.SET_PERSISTENCE(ref r);
	}
	public static void Load(this GameObject o, string FileName)
	{
		List<MonoPersistenceModel> r = new List<MonoPersistenceModel>();
		r = FileName.GET_PERSISTENCE(ref r);
		r.ForEach(n => o.GetComponent(n.Tipo).OVERWRITTE(n.MonoJson));
	}
	public static T Instantiate<T>(this T prefab) where T : Component
	{
		var g = GameObject.Instantiate(prefab);
		return g;
	}
	public static T Instantiate<T>(this T prefab, Vector3 p) where T : Component
	{
		var g = GameObject.Instantiate(prefab, p, Quaternion.identity);
		return g;
	}
	public static GameObject Instantiate(this GameObject prefab)
	{
		var g = GameObject.Instantiate(prefab);
		return g;
	}
}
public static class OriginableMethods
{
	public static void GetOrigin<T>(this T o, ref T receptor)
	{
		receptor = o;
	}
}
public class Methods<T>
{
	private List<SingleMethod<T>> l = new List<SingleMethod<T>>();
	public void Invoke (object invoker, T data)
	{
		l = new List<SingleMethod<T>>(l.OrderBy(n => n.index));
		object r = null;
		foreach(var n in l)
		{
			r = n.metodo.Invoke(invoker, data, r);
		}
	}
	public void add (SingleMethod<T> b)
	{
		l.ADD_NO_REPEAT(b, n => n.metodo == b.metodo || n.index == b.index);
	}
	public void remove(SingleMethod<T> b)
	{
		l.RemoveAll(n => n.metodo == b.metodo || n.index == b.index);
	}
	public static Methods<T> operator +(Methods<T> a, SingleMethod<T> b)
	{
		a.add(b);
		return a;
	}
	public static Methods<T> operator -(Methods<T> a, SingleMethod<T> b)
	{
		a.remove(b);
		return a;
	}
}
public delegate object Metodo<T>(object invoker, T data, object returned);
public class SingleMethod<T>
{
	public int index;
	public Metodo<T> metodo;
	public SingleMethod(int i, Metodo<T> m)
	{
		index = 0;
		metodo = m;
	}
}
[System.Serializable]
public class PermaVariable
{
    public string fileName;
    [SerializeField] protected Variable _fileContent;
    public Variable fileContent
    {
        get
        {
            return _fileContent;
        }
        set
        {
            _fileContent = value;
            variable = _fileContent;
        }
    }
	protected Variable variable
    {
		get
        {
			return fileName.GET_STRING();
        }
		set
        {
			fileName.SET_STRING(value);
        }
    }
    public PermaVariable(string File_Name)
    {
        fileName = File_Name;
        _fileContent = variable;
    }
	public void Save ()
	{
		fileContent = this;
	}
	public static implicit operator string(PermaVariable o)
	{
		return o.fileContent;
	}
	public static implicit operator int(PermaVariable o)
	{
		return o.fileContent;
	}
	public static implicit operator double(PermaVariable o)
	{
		return o.fileContent;
	}
	public static implicit operator float(PermaVariable o)
	{
		return o.fileContent;
	}
	public static implicit operator bool(PermaVariable o)
	{
		return o.fileContent;
	}
	public static implicit operator Variable(PermaVariable o)
	{
		return o.fileContent;
	}
}
[System.Serializable]
public class PermaObject<T>
{
    public string FileName;
    [SerializeField] protected T _fileContent;
    public T fileContent
    {
        get
        {
            return _fileContent;
        }
        set
        {
            _fileContent = value;
            variable = _fileContent;
        }
    }
	public bool isFirsLoad;
	protected T variable
    {
		get
        {
			if(FileName.EXIST_PERSISTENCE())
			{
				_fileContent = _fileContent.NEW();
				return FileName.GET_PERSISTENCE(ref _fileContent);
			}
			else
			{
				isFirsLoad = true;
				return _fileContent.NEW();
			}
        }
		set
        {
			FileName.SET_PERSISTENCE(value);
        }
    }
	public PermaObject(){}
    public PermaObject(string File_Name)
    {
        FileName = File_Name;
        _fileContent = variable;
    }
	public void Save ()
	{
		fileContent = this;
	}
	public static implicit operator T(PermaObject<T> o)
	{
		return o.fileContent;
	}
}
[System.Serializable]
public class PermaList<T>
{
    public string FileName;
    [SerializeField] protected List<T> _fileContent;
    public List<T> fileContent
    {
        get
        {
            return _fileContent;
        }
        set
        {
            _fileContent = value;
            variable = _fileContent;
        }
    }
	public bool isFirsLoad;
	protected List<T> variable
    {
		get
        {
			if(FileName.EXIST_PERSISTENCE())
			{
				_fileContent = _fileContent.NEW();
				return FileName.GET_PERSISTENCE(ref _fileContent);
			}
			else
			{
				isFirsLoad = true;
				return _fileContent.NEW();
			}
        }
		set
        {
			FileName.SET_PERSISTENCE(value);
        }
    }
	public PermaList(){}
    public PermaList(string File_Name)
    {
        FileName = File_Name;
        _fileContent = variable;
    }
	public void Save ()
	{
		fileContent = this;
	}
	public static implicit operator List<T>(PermaList<T> o)
	{
		return o.fileContent;
	}
}
public static class VariableMethods
{
	/*
	public static Variable RandomMinMax (this object min, Variable max)
	{
		return UnityEngine.Random.Range(min.ToString().TO_INT(), (int)(max + 1));
	}
	*/
	public static Variable RandomMinMax (this float min, Variable max)
	{
		return UnityEngine.Random.Range(min, max);
	}
	public static Variable RandomMinMax (this int min, Variable max)
	{
		return UnityEngine.Random.Range(min, max + 1);
	}
	public static int RandomCount (this int min, int max)
	{
		return UnityEngine.Random.Range(min, max);
	}
}
public static class FormatMethod
{
	public static string TO_FORMAT(this string o, params object [] p)
    {
		return string.Format(o, p);
    }
}
public static class MoveMethods
{
	public static void Move<T>(this IList<T> Source, T old, int newIndex)
	{
		Source.Remove(old);
		Source.Insert(newIndex, old);	
	}
	public static void MoveAll<T>(this IList<T> Source, Func<T, bool> expresion, int PlusIndex)
	{
		var s = Source.Where(expresion);

		if (PlusIndex < 0)
		{
			foreach (var n in s.OrderBy(w => Source.IndexOf(w)))
			{
				if (n != null)
				{
					Source.Move(n, Source.IndexOf(n) + PlusIndex);
				}
			}
		}
		else if (PlusIndex > 0)
		{
			foreach (var n in s.OrderByDescending(w => Source.IndexOf(w)))
			{
				if (n != null)
				{
					Source.Move(n, Source.IndexOf(n) + PlusIndex);
				}
			}
		}
	}
}
public static class WebPage
{
	/// <summary>
	/// 
	/// </summary>
	/// <param name="link">https://</param>
	/// <param name="headers">content-type: application/json (abreviado -H de -Header)</param>
	/// <param name="data">data["client_id"] = "123456789" (abreviado -d de data)</param>
	/// <param name="GET_POST_PUT_PATCH_DELETE"></param>
	/// <returns></returns>
	public static async Task<string> POSTMAN (this string link, string[] headers, NameValueCollection data = null, string GET_POST_PUT_PATCH_DELETE = "GET")
    {
		if(data == null)
        {
			return await link.DOWNLOAD(headers);
        }

		var wb = new WebClient();
		if (headers != null)
		{
			foreach (var h in headers)
			{
				if (h == null)
				{
					continue;
				}
				wb.Headers.Add(h);
			}
		}

		var byteResponse = await wb.UploadValuesTaskAsync(link, GET_POST_PUT_PATCH_DELETE, data);
		return Encoding.Default.GetString(byteResponse);
	}
	public static async Task<string> POSTMAN<T>(this string link, string[] headers, T data, string GET_POST_PUT_PATCH_DELETE = "GET")
	{
		if (data == null)
		{
			return await link.DOWNLOAD(headers);
		}

		var wb = new WebClient();
		if (headers != null)
		{
			foreach (var h in headers)
			{
				if (h == null)
				{
					continue;
				}
				wb.Headers.Add(h);
			}
		}
		return await wb.UploadStringTaskAsync(link, GET_POST_PUT_PATCH_DELETE, data.TO_JSON());
	}
	/// <summary>
	/// web download
	/// </summary>
	/// <param name="link">https://</param>
	/// <param name="path">C://</param>
	/// <param name="headers">content-type: application/json</param>
	/// <returns></returns>
	private static async Task<string> DOWNLOAD(this string link, string[] headers)
	{
		using (var wc = new WebClient())
		{
			//wc.UseDefaultCredentials = true;
			//wc.Credentials = CredentialCache.DefaultNetworkCredentials;
			//wc.Headers.Add("Content-Type", "application/json");

			if(headers != null)
            {
				foreach(var h in headers)
                {
					if(h == null)
                    {
						continue;
                    }
					wc.Headers.Add(h);
				}
            }
			return await wc.DownloadStringTaskAsync(@link);
		}
	}
}
public class Owner<T>
{
	public object The_Owner;
	public List<T> The_List;

	public static Owner<T> CREATE(ref object owner, List<T> be)
	{
		return new Owner<T>() { The_Owner = owner, The_List = be };
	}
}
public class folder_file
{
	public string folder;
	public string file;

	public folder_file(string fo, string fi)
	{
		folder = fo;
		file = fi;
	}
}
public static class Valores
{
	public static string IE_TO_JSON (this IEnumerable o)
    {
		string r = "";
		foreach(var n in o)
		{
			if(n == null) { continue; }
			r += n.TO_JSON() + "\n\n";
		}
		return r;
    }
	public static string IE_TO_STRING (this IEnumerable o , string separador = "")
    {
		

		string r = "";
		foreach(var n in o)
		{
			if(n == null) { continue; }
			if(separador.IS_NULL())
			{
				r += n + "\n\n";
			}
			else
			{
				
				r += n + separador + "\n\n";
			}
		}
		return r;
    }
	public static string BYTE_TO_STRING(this byte[] o)
	{
		var r = "";
		foreach(var c in o)
		{
			r += c;
		}
		return r;
	}

	public static folder_file TO_FOLDER_FILE(this string o, string[] character)
	{

		var l = o.Splitting(character);

		if (l.Count <= 1)
		{
			return new folder_file("", o);
		}

		var FILE = l[l.Count - 1];
		l.Reverse();
		l.Remove(FILE);
		string FOLDER = "";
		l.Reverse();
		foreach (var n in l)
		{
			if (n != null)
			{
				if (n == l[l.Count - 1])
				{
					FOLDER += n;
				}
				else
				{
					FOLDER += n + character;

				}
			}
		}
		return new folder_file(FOLDER, FILE);
	}
	public static List<string> FOLDER_DIR(this string o)
	{
		return new List<string>(Directory.GetDirectories(o.START_FOLDER()));
	}
	public static List<string> FILES_DIR(this string o)
	{
		return new List<string>(Directory.GetFiles(o.START_FOLDER()));
	}
	public static void InvokeMethod (this object o, string MethodName, params object[] parameters)
    {
		o.GetType().GetMethod(MethodName)?.Invoke(o, parameters);
    }
	public static T GET_PROPERTY<T> (this object o, string name)
	{
		return (T)o.GetType().GetProperty(name).GetValue(o);
	}
	public static void SET_PROPERTY(this object o, string name, object value)
	{
		o.GetType().GetProperty(name).SetValue(o, value);
	}
	public static Owner<PropertyInfo> PROPERTYS(this object o)
	{
		return Owner<PropertyInfo>.CREATE(ref o, new List<PropertyInfo>(o.GetType().GetProperties()).FindAll(n => n.CanRead || n.CanWrite));
	}
	public static Owner<FieldInfo> FIELDS(this object o)
	{
		return Owner<FieldInfo>.CREATE(ref o, new List<FieldInfo>(o.GetType().GetFields()));
	}
	public static List<string> Splitting(this object o, string[] Spliters)
	{
		return new List<string>((o + "").Split(Spliters, StringSplitOptions.RemoveEmptyEntries));
	}
	public static object MORPH(this object ToType, string value)
	{
		if (ToType is bool)
		{
			return value.TO_BOOL();
		}
		if (ToType is int)
		{
			return value.TO_INT();
		}
		if (ToType is float)
		{
			return value.TO_FLOAT();
		}
		if (ToType is double)
		{
			return value.TO_DOUBLE();
		}
		if (ToType is Morphim)
		{
			return value.TO_VARIABLE();
		}
		if (ToType is DateTime)
		{
			return value.TO_DATE_TIME();
		}
		if (ToType is Enum)
		{
			return Enum.Parse(ToType.GetType(), value);
		}
		if (ToType is string)
		{
			return value;
		}
		throw new Exception(ToType.GetType().Name + " dont handled");
	}
	public static string GET_VALUE(this object o, string Variable_Name)
	{
		var f = o.FIELDS().The_List.Find(n => n.Name == Variable_Name);
		var p = o.PROPERTYS().The_List.Find(n => n.Name == Variable_Name);

		if (p != null)
		{
			return p.GetValue(o, new object[] { }) + "";
		}
		else if (f != null)
		{
			return f.GetValue(o) + "";
		}
		throw new Exception(Variable_Name + " dont exist");
	}
	public static bool SET_VALUE(this object o, string Variable_Name, string value)
	{
		var f = o.FIELDS().The_List.Find(n => n.Name == Variable_Name);
		var p = o.PROPERTYS().The_List.Find(n => n.Name == Variable_Name);

		if (p != null)
		{
			var v = p.GetValue(o, new object[] { });
			if (v == null)
			{
				return false;
			}
			p.SetValue(o, v.MORPH(value), new object[] { });
			return true;
		}
		else if (f != null)
		{
			var v = f.GetValue(o);
			if (v == null)
			{
				return false;
			}
			f.SetValue(o, v.MORPH(value));
			return true;
		}
		return false;
	}
	public static List<T> REINDEXING<T>(this List<T> o, string Variable_Name)
	{
		for (int i = 0; i < o.Count; i++)
		{
			o[i].SET_VALUE(Variable_Name, i.ToString());
		}
		return o;
	}
}
public static class Ser
{
	public static string COMBINE (this string a, string b)
    {
		return Path.Combine(a, b);
    }
	public static void SET_BYTES(this string path, byte[] value)
	{
		if (Folder.IS_NULL())
		{
			throw new Exception("SPECIAL FOLDER IS NULL : use string.START_SPECIAL_FOLDER() before");
		}
		var r = path.CREATE_PATH(new string[] { "/", @"\" });
		if (File.Exists(r))
		{
			File.WriteAllBytes(r, value);
		}
		else
		{
			File.WriteAllBytes(r, value);
		}
	}
	public static string CREATE_PATH(this string path, string[] separator)
	{
        try
        {
			var w = path.Split(separator, StringSplitOptions.RemoveEmptyEntries);
			var fold = "";
			for (int i = 0; i < w.Length; i++)
			{
				var c = w[i];
				if (i == w.Length - 1)
				{
					fold = Path.Combine(fold, c + ".txt");
				}
				else
				{
					fold = Path.Combine(fold, c);
					//MyCopyNote.Services.Dbugger.Log(fold);

					try
					{
						if (!Directory.Exists(Path.Combine(Folder, fold)))
						{
							Directory.CreateDirectory(Path.Combine(Folder, fold));
						}
					}
					catch (Exception ex)
					{
						Debug.LogError(ex);
					}
				}
			}
			return Path.Combine(Folder, fold);
		}
		catch(Exception ex)
        {
			throw new Exception(ex.ToString());
        }
	}
	public static string START_FOLDER(this string o)
	{
		if (Folder.IS_NULL())
		{
			throw new Exception("SPECIAL FOLDER IS NULL : use string.START_SPECIAL_FOLDER() before");
		}

		if (o.IS_NULL())
		{
			return Folder;
		}
		else
		{
			var r = Path.Combine(Folder, o);
			if (Directory.Exists(r))
			{
				return r;
			}
			else
			{
				Directory.CreateDirectory(r);
				return r;
			}
		}
	}
	public static string Folder { get; set; }
	public static string START_SPECIAL_FOLDER(this string o)
	{
		if (Folder.IS_NULL())
		{
			if (Directory.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), o)))
			{
				return Folder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), o);
			}
			else
			{
				Directory.CreateDirectory(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), o));
				return Folder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), o);
			}
		}
		else
		{
			return Folder;
		}
	}
	//EXIST PERSISTENCE
	public static bool EXIST_PERSISTENCE(this string path)
	{
		var r = Path.Combine(Folder, path + ".txt");
		return File.Exists(r);
	}


	//FIND PERSISTENCE
	public static T FIND_PERSISTENCE<T>(this string path, T o, Predicate<T> p)
	{
		var r = new List<T>();
		path.GET_PERSISTENCE(ref r);
		return r.Find(p);
	}
	public static T FIND_PERSISTENCE<T>(this string path, Predicate<T> p)
	{
		var r = new List<T>();
		path.GET_PERSISTENCE(ref r);
		return r.Find(p);
	}

	//SELECT PERSISTENCE
	public static List<T> SELECT_PERSISTENCE<T>(this string path, T o, Predicate<T> p)
	{
		var r = new List<T>();
		path.GET_PERSISTENCE(ref r);
		return r.FindAll(p);
	}
	//SELECT PERSISTENCE
	public static List<T> SELECT_PERSISTENCE<T>(this string path, Predicate<T> p)
	{
		var r = new List<T>();
		path.GET_PERSISTENCE(ref r);
		return r.FindAll(p);
	}

	//ADD PERSISTENCE
	public static List<T> ADD_PERSISTENCE<T>(this string path, T o, Predicate<T> p = null)
	{
		var r = new List<T>();
		path.GET_PERSISTENCE(ref r);

		if (p == null)
        {
			r.Insert(0, o);
			path.SET_PERSISTENCE(r);
			return r;
		}

		if (r.Exists(p))
		{
			r.RemoveAll(p);
			r.Insert(0, o);
		}
		else
		{
			r.Insert(0, o);
		}
		path.SET_PERSISTENCE(r);
		return r;
	}
	public static void ADD_PERSISTENCE_RAW(this string path, string json)
	{
		var r = path.GET_STRING_BY_LINES();
		r.Add(json);
		path.SET_STRING_BY_LINES(r);
	}

	//REMOVE PERSISTENCE
	public static List<T> REMOVE_PERSISTENCE<T>(this string path, T o, Predicate<T> p)
	{
		var r = new List<T>();
		path.GET_PERSISTENCE(ref r);
		r.RemoveAll(p);
		path.SET_PERSISTENCE(r);
		return r;
	}
	//REMOVE PERSISTENCE
	public static List<T> REMOVE_PERSISTENCE<T>(this string path, Predicate<T> p)
	{
		var r = new List<T>();
		path.GET_PERSISTENCE(ref r);
		r.RemoveAll(p);
		path.SET_PERSISTENCE(r);
		return r;
	}




	//GET PERSISTENCE
	public static List<T> GET_PERSISTENCE<T>(this string path, ref List<T> o)
	{
		return o = o.GET_FILE(path).TO_OBJECT(ref o);
	}
	public static List<T> GET_PERSISTENCE<T>(this string path, List<T> o)
	{
		return o = o.GET_FILE(path).TO_OBJECT(ref o);
	}
	public static T GET_PERSISTENCE<T>(this string path, ref T o)
	{
		return o = o.GET_FILE(path).TO_OBJECT(ref o);
	}
	public static T GET_PERSISTENCE<T>(this string path, T o)
	{
		return o = o.GET_FILE(path).TO_OBJECT<T>();
	}

	//SET PERISTENCE
	public static void SET_PERSISTENCE<T>(this string path, ref List<T> o)
	{
		o.SET_FILE(path);
	}
	public static void SET_PERSISTENCE<T>(this string path, List<T> o)
	{
		o.SET_FILE(path);
	}
	public static void SET_PERSISTENCE<T>(this string path, ref T o)
	{
		o.SET_FILE(path);
	}
	public static void SET_PERSISTENCE<T>(this string path, T o)
	{
		o.SET_FILE(path);
	}
	//COPY FILE
	public static void COPY_FILE(this string o, string to)
	{
		o = Path.Combine(Folder, o + ".txt");
		to = Path.Combine(Folder, (to + ".txt").Replace(":", "-"));
		File.Copy(o, to);
	}
	//DELETE FILE
	public static bool DELETE_FILE(this string path)
	{
		if (Folder.IS_NULL())
		{
			throw new Exception("SPECIAL FOLDER IS NULL : use string.START_SPECIAL_FOLDER() before");
		}

		var r = Path.Combine(Folder, path + ".txt");

		if (File.Exists(r))
		{
			File.Delete(r);
			return true;
		}
		return false;
	}
	//GET FILE
	private static List<string> GET_FILE<T>(this List<T> o, string path)
	{
		try
        {
			if (Folder.IS_NULL())
			{
				throw new Exception("SPECIAL FOLDER IS NULL : use string.START_SPECIAL_FOLDER() before");
			}

			var r = path.CREATE_PATH(new string[] { "/", @"\" });

			if (File.Exists(r))
			{
				return new List<string>(File.ReadAllLines(r));
			}
			else
			{
				File.Create(r);
				return new List<string>();
			}
		}
		catch(Exception ex)
        {
			Debug.LogError(ex);
			return new List<string>();
        }
	}
	public static void SET_STRING(this string path, string data)
    {
		if (Folder.IS_NULL())
		{
			throw new Exception("SPECIAL FOLDER IS NULL : use string.START_SPECIAL_FOLDER() before");
		}
		var r = path.CREATE_PATH(new string[] { "/", @"\" });
		File.WriteAllText(r, data);
	}
	public static void SET_STRING_BY_LINES(this string path, IEnumerable<string> data)
    {
		if (Folder.IS_NULL())
		{
			throw new Exception("SPECIAL FOLDER IS NULL : use string.START_SPECIAL_FOLDER() before");
		}
		var r = path.CREATE_PATH(new string[] { "/", @"\" });
		File.WriteAllLines(r, data);
	}
	public static string GET_STRING(this string path)
	{
		if (Folder.IS_NULL())
		{
			throw new Exception("SPECIAL FOLDER IS NULL : use string.START_SPECIAL_FOLDER() before");
		}

		try
        {
			var r = path.CREATE_PATH(new string[] { "/", @"\" });

			if (File.Exists(r))
			{
				return File.ReadAllText(r);
			}
			else
			{
				File.Create(r);
				return "";
			}
		}
        catch (Exception ex)
        {
			Debug.LogError(ex);
			return "";
        }
	}
	public static List<string> GET_STRING_BY_LINES (this string path)
	{
		if (Folder.IS_NULL())
		{
			throw new Exception("SPECIAL FOLDER IS NULL : use string.START_SPECIAL_FOLDER() before");
		}

		try
        {
			var r = path.CREATE_PATH(new string[] { "/", @"\" });

			if (File.Exists(r))
			{
				return new List<string>(File.ReadAllLines(r));
			}
			else
			{
				File.Create(r);
				return new List<string>(new string[] { });
			}
		}
        catch (Exception ex)
        {
			Debug.LogError(ex);
			return new List<string>(new string[] { });
        }
	}
	private static string GET_FILE<T>(this T o, string path)
	{
		if (Folder.IS_NULL())
		{
			throw new Exception("SPECIAL FOLDER IS NULL : use string.START_SPECIAL_FOLDER() before");
		}

		var r = path.CREATE_PATH(new string[] { "/", @"\" });

		if (File.Exists(r))
		{
			return File.ReadAllText(r);
		}
		else
		{
			File.Create(r);
			return "";
		}
	}
	//SET FILE
	private static void SET_FILE<T>(this T o, string path)
	{
		if (Folder.IS_NULL())
		{
			throw new Exception("SPECIAL FOLDER IS NULL : use string.START_SPECIAL_FOLDER() before");
		}
		var r = path.CREATE_PATH(new string[] { "/", @"\" });
		File.WriteAllText(r, o.TO_JSON());
	}
	private static void SET_FILE<T>(this List<T> o, string path)
	{
		if (Folder.IS_NULL())
		{
			throw new Exception("SPECIAL FOLDER IS NULL : use string.START_SPECIAL_FOLDER() before");
		}
		var r = path.CREATE_PATH(new string[] { "/", @"\" });
		File.WriteAllLines(r, o.TO_JSON().ToArray());
	}
	//SINGLE OBJECT
	public static string TO_JSON_RAW(this object o)
	{
		return JsonUtility.ToJson(o);
	}
	public static string TO_JSON<T>(this T o)
	{
		return JsonUtility.ToJson(o);
	}
	public static void OVERWRITTE(this object o, string JSON)
	{
		JsonUtility.FromJsonOverwrite(JSON, o);
	}
	public static T TO_OBJECT<T>(this string JSON, ref T o)
	{
		JsonUtility.FromJsonOverwrite(JSON, o);
		return o;
	}
	public static T TO_OBJECT<T>(this string JSON, T o)
	{
		JsonUtility.FromJsonOverwrite(JSON, o);
		return o;
	}
	public static T TO_OBJECT<T>(this string JSON)
	{
		return JsonUtility.FromJson<T>(JSON);
	}

	//LIST OBJECT
	public static string TO_JSON_LIST<T>(this List<T> o)
	{
		string r = "";
		o.ForEach(n => r += n.TO_JSON_RAW() + "(J?)");
		return r;
	}
	public static List<T> TO_OBJECT_LIST<T>(this string o, ref List<T> receptor)
	{
		List<T> r = new List<T>();
		List<string> s = o.Splitting(new string[] {"(J?)"});
		r = s.ConvertAll(n => n.TO_OBJECT<T>());
		return r;
	}
	public static List<T> TO_OBJECT_LIST<T>(this string o, List<T> receptor)
	{
		List<T> r = new List<T>();
		List<string> s = o.Splitting(new string[] {"(J?)"});
		r = s.ConvertAll(n => n.TO_OBJECT<T>());
		return r;
	}
	public static List<string> TO_JSON<T>(this List<T> o)
	{
		return o.ConvertAll(n => n.TO_JSON());
	}
	public static List<T> TO_OBJECT<T>(this List<string> JSONs, ref List<T> o)
	{
		return o = JSONs.ConvertAll(n => n.TO_OBJECT<T>());
	}
	public static List<T> TO_OBJECT<T>(this string[] JSONs, ref List<T> o)
	{
		var j = new List<string>(JSONs);
		return o = j.ConvertAll(n => n.TO_OBJECT<T>());
	}
	public static bool IS_NULL(this object o)
	{
		return string.IsNullOrEmpty(o + "") || string.IsNullOrWhiteSpace(o + "");
	}
	public static bool IS_FULL(this object o)
	{
		return !o.IS_NULL();
	}
	public static bool TO_BOOL(this string o)
	{
		if (o.IS_NULL())
		{
			throw new Exception("Morph Empty string exception");
		}
		else
		{
			return bool.Parse(o);
		}
	}
	public static int TO_INT(this string o)
	{
		if (o.IS_NULL())
		{
			return 0;
		}
		else
		{
			return (int)double.Parse(o);
		}
	}
	public static float TO_FLOAT(this string o)
	{
		if (o.IS_NULL())
		{
			return 0f;
		}
		else
		{
			return float.Parse(o);
		}
	}
	public static double TO_DOUBLE(this string o)
	{
		if (o.IS_NULL())
		{
			return 0;
		}
		else
		{
			return double.Parse(o);
		}
	}
	public static Morphim TO_VARIABLE(this string o)
	{
		if (o.IS_NULL())
		{
			return "";
		}
		else
		{
			return o;
		}
	}
	public static DateTime TO_DATE_TIME(this string o)
	{
		if (o.IS_NULL())
		{
			throw new Exception("Morph Empty string exception");
		}
		else
		{
			return DateTime.Parse(o);
		}
	}
	/// <summary>
	/// (T)new object()
	/// </summary>
	/// <typeparam name="T">The 1st type parameter.</typeparam>
	public static T NEW<T>(this object o)
	{
		return Activator.CreateInstance<T>();
	}
	/// <summary>
	/// (T)new object()
	/// </summary>
	/// <param name="o">O.</param>
	/// <typeparam name="T">The 1st type parameter.</typeparam>
	public static T NEW<T>(this T o)
	{
		return Activator.CreateInstance<T>();
	}
	static int voids = 0;
	public static Stream GET_VOID_STREAM<T>(this string path, T data)
    {
		(path + voids).SET_PERSISTENCE(data);
		return (path + voids++).GET_STREAM();
    }
	/// <summary>
	/// use a full directory link string
	/// </summary>
	/// <param name="o"></param>
	/// <returns></returns>
	public static Stream GET_RAW_STREAM(this string o)
	{
		return File.OpenRead(o);
	}
	/// <summary>
	/// use a short directory link string
	/// </summary>
	/// <param name="o"></param>
	/// <returns></returns>
	public static Stream GET_STREAM(this string o)
	{
		if (Folder.IS_NULL())
		{
			throw new Exception("SPECIAL FOLDER IS NULL : use string.START_SPECIAL_FOLDER() before");
		}
		var r = Path.Combine(Folder, o + ".txt");
		return File.OpenRead(r);
	}
	public static int Count(this IEnumerable o)
	{
		int r = 0;
		foreach (var n in o)
		{
			r++;
		}
		return r;
	}
		public static void INSERT_NO_REPEAT<T>(this List<T> l, int index, T add, Predicate<T> no_repeat)
	{
		if(l.Exists(no_repeat))
		{

		}
		else
		{
			l.Insert(index, add);
		}
	}
	public static void ADD_NO_REPEAT<T>(this List<T> l, T add, Predicate<T> no_repeat)
	{
		if(l.Exists(no_repeat))
		{

		}
		else
		{
			l.Add(add);
		}
	}
	public static void ADD_OR_REPLACE<T>(this List<T> l, T add, Predicate<T> no_repeat)
	{
		var i = l.FindIndex(no_repeat);
		if(i == -1)
		{
			l.Add(add);
		}
		else
		{
			l[i] = add;
		}
	}
	public static List<T> ArrayToList<T> (this T[] o)
	{
		return new List<T>(o);
	}
	public static void FOR_EACH<T>(this Transform o, params Action<T>[] a) where T : Component
	{
		var r = new List<T>();
		foreach(Transform c in o)
		{
			if(c == null){ continue; }
			r.Add(c.GetComponent<T>());
		}
		r.ForEach(n => a.ArrayToList().ForEach(q => q.Invoke(n)));
	}
	public static List<T> FIND_ALL<T>(this Transform o, Predicate<T> p) where T : Component
	{
		var r = new List<T>();
		foreach(Transform c in o)
		{
			if(c == null){ continue; }
			var S = c.GetComponent<T>();
			r.Add(S);
		}
		return r.FindAll(p);
	}
	public static List<T> FIND_EVERYONE<T>(this Transform o, Predicate<T> p) where T : Component
	{
		var r = new List<T>();

		r = new List<T>(o.GetComponentsInChildren<T>(true));

		if(o is T)
		{
			r.RemoveAll(n => n == o);
		}

		return r.FindAll(p);
	}
	public static List<Component> FIND_ALL(this Transform o, Predicate<Component> p, string tipo)
	{
		var r = new List<Component>();
		foreach(Transform c in o)
		{
			if(c == null){ continue; }
			var S = c.GetComponent(tipo);
			r.Add(S);
		}
		return r.FindAll(p);
	}
	public static List<Component> FIND_EVERYONE(this Transform o, Predicate<Component> p, string tipo)
	{
		var r = new List<Component>();

		r = new List<Component>(o.GetComponentsInChildren<Component>(true));

		r.RemoveAll(n => n == o);

		r = r.FindAll(n => n.GetComponent(tipo) != null);

		return r.FindAll(p);
	}
	public static T FIND<T>(this Transform o, Predicate<T> p) where T : Component
	{
		var r = new List<T>();
		foreach(Transform c in o)
		{
			if(c == null){ continue; }
			var S = c.GetComponent<T>();
			r.Add(S);
		}
		return r.Find(p);
	}
	public static void DESTROY_ALL_GAMEOBJECT<T>(this Transform o, Predicate<T> p) where T : Component
	{
		var r = o.FIND_ALL<T>(p);
		r.ForEach(n => GameObject.Destroy(n.gameObject, 0f));
	}
	public static void DESTROY_ALL_COMPONENT<T>(this Transform o, Predicate<T> p) where T : Component
	{
		var r = o.FIND_ALL<T>(p);
		r.ForEach(n => GameObject.Destroy(n, 0f));
	}
	public static string TYPE(this object o)
	{
		return o.GetType().Name;
	}
}
public static class MethodMethods
{
	public static bool EN_CAMBIO<Q> (this Q o, ref Q diferente, params Action[] metodos)
	{
		var b = o.ToString() != diferente.ToString();
		if(b)
		{
			foreach(var c in metodos)
			{
				if(c == null){ continue;}
				c.Invoke();
			}
		}
		diferente = o;
		return b;
	}
	///<summary>
	/// aplica el metodo al script o la clase a la que pertenece este objeto
	///</summary>
	public static bool EN_CAMBIO<Q, T> (this Q o, ref Q diferente, params Action<T>[] metodos)
	{
		var b = o.ToString() != diferente.ToString();
		if(b)
		{
			foreach(var c in metodos)
			{
				if(c == null){ continue;}
				c.Invoke((T)c.Target);
			}
		}
		diferente = o;
		return b;
	}
}

	[System.Serializable]
	public class PersistenceVariable
    {
		public string fileName { get; set; }
		public Variable variable
        {
			get
            {
				return fileName.GET_STRING();
            }
			set
            {
				fileName.SET_STRING(value);
            }
        }
		public PersistenceVariable (string File_Name) 
		{
			fileName = File_Name;
		}
    }
		[System.Serializable]
	public class PersistenceObject<T>
    {
		public string fileName { get; set; }
		public T variable
        {
			get
            {
				T o = default;
				return fileName.GET_PERSISTENCE(o);
            }
			set
            {
				fileName.SET_PERSISTENCE(value);
            }
        }
		public PersistenceObject (string File_Name) 
		{
			fileName = File_Name;
		}
    }
	[System.Serializable]
	public class Variable : EventArgs
    {
		public Variable() { }
		public Variable(object o)
        {
			Value = o.ToString();
        }
		public string Value;
        public override string ToString()
        {
			return Value;
        }

        public static implicit operator Variable(string o)
		{
			return new Variable(o);
		}
		public static implicit operator Variable(int o)
		{
			return new Variable(o);
		}
		public static implicit operator Variable(float o)
		{
			return new Variable(o);
		}
		public static implicit operator Variable(double o)
		{
			return new Variable(o);
		}
		public static implicit operator Variable(bool o)
		{
			return new Variable(o);
		}
		public static implicit operator Variable(DateTime o)
		{
			return new Variable(o);
		}

		public static implicit operator string(Variable o)
		{
			if (o.IS_NULL())
			{
				if (o == null)
				{
					throw new Exception("Null Variable");
				}
				else
				{
					return o.ToString();
				}
			}
			else
			{
				return o.ToString();
			}
		}
		public static implicit operator int(Variable o)
		{
			return o.ToString().TO_INT();
		}
		public static implicit operator float(Variable o)
		{
			return o.ToString().TO_FLOAT();
		}
		public static implicit operator double(Variable o)
		{
			return o.ToString().TO_DOUBLE();
		}
		public static implicit operator bool(Variable o)
		{
			return o.ToString().TO_BOOL();
		}
		public static implicit operator DateTime(Variable o)
		{
			return o.ToString().TO_DATE_TIME();
		}


	}
	public class Morphim : IEnumerable<Morphim>
	{
		/// <summary>
		/// VariableSymbol
		/// </summary>
		public const string V = "|V|";
		/// <summary>
		/// ValueSymbol
		/// </summary>
		public const string E = ":=:";

		public static System.StringSplitOptions RemoveEmpty
		{
			get
			{
				return System.StringSplitOptions.RemoveEmptyEntries;
			}
		}
		public string Split(int o)
		{
			var w = Value.Split(new string[] { E }, RemoveEmpty);
			if (o < w.Count())
			{
				return w[o].Replace(V, "");
			}
			else
			{
				return "";
			}
		}
		public int Count { get { if (_cache == null) { _cache = new List<Morphim>(); } return Cache.Count; } }
		public Morphim Name { get { return Split(0); } }
		public Morphim Quality { get { return Split(1); } }

		public string Value { get; set; }

		private string Memo { get; set; }
		public bool isChanged
		{
			get
			{
				if (Value != Memo)
				{
					Memo = Value;
					return true;
				}
				return false;
			}
			set
			{
				if (value)
				{
					Memo = Value;
				}
			}
		}
		public Morphim(object o)
		{
			Value = "";
			Value = o + "";
		}
		public Morphim(string o)
		{
			Value = "";
			Value = o;
		}
		public override string ToString()
		{
			return Value;
		}
		public Morphim StartAdd
		{
			get
			{
				Value = "";
				return this;
			}
		}
		public Morphim Add(object name, object value, string opcional)
		{
			Value += name + E + value + V + opcional;
			return this;
		}
		public Morphim Add(object name, object value)
		{
			Value += name + E + value + V;
			return this;
		}
		public Morphim Add(object name, Morphim value)
		{
			Value += name + E + value + V;
			return this;
		}
		public Morphim Add(Morphim value)
		{
			Value += value;
			return this;
		}
		private string MemoCache = "";
		private List<Morphim> _cache = new List<Morphim>();
		public List<Morphim> Cache
		{
			get
			{
				IsCacheTime();
				return _cache;
			}
		}
		public bool IsCacheTime()
		{
			if (Value != MemoCache)
			{
				var L = new List<string>(Value.Split(new string[] { V }, RemoveEmpty));
				_cache = L.ConvertAll(n => new Morphim(n + V));
				MemoCache = Value;
				return true;
			}
			return false;
		}

		public IEnumerator<Morphim> GetEnumerator()
		{
			return Cache.GetEnumerator();
		}
		IEnumerator IEnumerable.GetEnumerator()
		{
			return Cache.GetEnumerator();
		}
		public Morphim this[int i]
		{
			get
			{
				return Cache[i];//improve
			}
		}
		public Morphim this[string name, Morphim value]
		{
			get
			{
				if (string.IsNullOrEmpty(name))
				{
					//Dbugger.Log("name is null");
					return "name = null";
				}
				else if (Value == null)
				{
					//Dbugger.Log("Value == null");
					return "Value = null";
				}
				else if (value.IS_NULL())
				{
					//Dbugger.Log("value.isNull");
					return "value.isNull";
				}
				else if (Cache.Exists(n => n.Name == name))
				{
					//Cache.ForEach(n => Dbugger.Log(n.Name + " == " + name));
					Value = Value.Replace(Cache.Find(n => n.Name == name + ""), name + E + value + V);
					return Cache.Find(n => n.Name == name).Quality;
				}
				else
				{
					Add(name + "", value);
					return Cache.Find(n => n.Name == name).Quality;
				}
			}
		}
		public Morphim Replace(string name, Morphim value)
		{
			if (string.IsNullOrEmpty(name))
			{
				//Dbugger.Log("name is null");
				return "name = null";
			}
			else if (Value == null)
			{
				//Dbugger.Log("Value == null");
				return "Value = null";
			}
			else if (value.IS_NULL())
			{
				//Dbugger.Log("value.isNull");
				return "value.isNull";
			}
			else if (Cache.Exists(n => n.Name == name))
			{
				//Cache.ForEach(n => Dbugger.Log(n.Name + " == " + name));
				Value = Value.Replace(Cache.Find(n => n.Name == name + ""), name + E + value + V);
				return this;
			}
			else
			{
				return this;
			}
		}
		public Morphim this[string name]
		{
			get
			{
				if (string.IsNullOrEmpty(name))
				{
					//Dbugger.Log("name is null");
					return "name = null";
				}
				else if (Value == null)
				{
					//Dbugger.Log("Value == null");
					return "Value = null";
				}
				else if (Cache.Exists(n => n.Name == name))
				{
					return Cache.Find(n => n.Name == name).Quality;
				}
				else
				{
					//Dbugger.Log("Name Dont Exist: " + name);
					return "";
				}
			}
		}
		//Variable
		public static implicit operator Morphim(string o)
		{
			return new Morphim(o);
		}
		public static implicit operator Morphim(int o)
		{
			return new Morphim(o);
		}
		public static implicit operator Morphim(float o)
		{
			return new Morphim(o);
		}
		public static implicit operator Morphim(double o)
		{
			return new Morphim(o);
		}
		public static implicit operator Morphim(bool o)
		{
			return new Morphim(o);
		}
		public static implicit operator Morphim(DateTime o)
		{
			return new Morphim(o);
		}
		public static implicit operator Morphim(List<Morphim> o)
		{
			Morphim r = new Morphim("");
			o.ForEach(n => r.Add(n));
			return r;
		}
		//Type
		public static implicit operator List<Morphim>(Morphim o)
		{
			return new List<Morphim>(o.Cache);
		}
		public static implicit operator string(Morphim o)
		{
			if (o.IS_NULL())
			{
				if (o == null)
				{
					throw new Exception("Null Variable");
				}
				else
				{
					return o.ToString();
				}
			}
			else
			{
				return o.ToString();
			}
		}
		public static implicit operator int(Morphim o)
		{
			return o.ToString().TO_INT();
		}
		public static implicit operator float(Morphim o)
		{
			return o.ToString().TO_FLOAT();
		}
		public static implicit operator double(Morphim o)
		{
			return o.ToString().TO_DOUBLE();
		}
		public static implicit operator bool(Morphim o)
		{
			return o.ToString().TO_BOOL();
		}
		public static implicit operator DateTime(Morphim o)
		{
			return o.ToString().TO_DATE_TIME();
		}
	}
