/*
 * Microsoft Reciprocal License (Ms-RL)
 * 
 * This license governs use of the accompanying software. If you use the software, 
 * you accept this license. If you do not accept the license, do not use the software.
 *
 * 1. Definitions
 * The terms "reproduce," "reproduction," "derivative works," and "distribution" 
 * have the same meaning here as under U.S. copyright law.
 * 
 * A "contribution" is the original software, or any additions or changes to the software.
 * A "contributor" is any person that distributes its contribution under this license.
 * "Licensed patents" are a contributor's patent claims that read directly on its contribution.
 * 
 * 2. Grant of Rights
 * 
 * (A) Copyright Grant- Subject to the terms of this license, including the license conditions 
 * and limitations in section 3, each contributor grants you a non-exclusive, worldwide, 
 * royalty-free copyright license to reproduce its contribution, prepare derivative works 
 * of its contribution, and distribute its contribution or any derivative works that you 
 * create.
 * 
 * (B) Patent Grant- Subject to the terms of this license, including the license conditions 
 * and limitations in section 3, each contributor grants you a non-exclusive, worldwide, 
 * royalty-free license under its licensed patents to make, have made, use, sell, offer for 
 * sale, import, and/or otherwise dispose of its contribution in the software or derivative 
 * works of the contribution in the software.
 * 
 * 3. Conditions and Limitations
 * 
 * (A) Reciprocal Grants- For any file you distribute that contains code from the software 
 * (in source code or binary format), you must provide recipients the source code to that 
 * file along with a copy of this license, which license will govern that file. You may 
 * license other files that are entirely your own work and do not contain code from the 
 * software under any terms you choose.
 * 
 * (B) No Trademark License- This license does not grant you rights to use any contributors' 
 * name, logo, or trademarks.
 * 
 * (C) If you bring a patent claim against any contributor over patents that you claim are 
 * infringed by the software, your patent license from such contributor to the software ends 
 * automatically.
 * 
 * (D) If you distribute any portion of the software, you must retain all copyright, patent, 
 * trademark, and attribution notices that are present in the software.
 * 
 * (E) If you distribute any portion of the software in source code form, you may do so only 
 * under this license by including a complete copy of this license with your distribution. 
 * If you distribute any portion of the software in compiled or object code form, you may 
 * only do so under a license that complies with this license.
 * 
 * (F) The software is licensed "as-is." You bear the risk of using it. The contributors give 
 * no express warranties, guarantees or conditions. You may have additional consumer rights 
 * under your local laws which this license cannot change. To the extent permitted under your 
 * local laws, the contributors exclude the implied warranties of merchantability, fitness 
 * for a particular purpose and non-infringement. 
 */

using System;
using System.Collections;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace Neural.Managers {
	/// <summary>
	/// The generic manager is a delegate for the particular managers.
	/// </summary>
	public class GenericManager {
		private string extension;
		private string vaultDirectory;
    
		/// <summary>
		/// Default Constructor.
		/// </summary>
		public GenericManager() {
		}
    
		/// <summary>
		/// Sets the file extension of the type of data to be stored.  Multiple types of
		/// files can share the same vault - but must have different extensions.
		/// </summary>
		public string Extension {
			set {
				extension = value;
			} 
			get {
				return extension;
			}
		}
    
    
		/// <summary>
		/// Add a managed object, passing in the object, the storage tag and the object
		/// name as a string.
		/// </summary>
		/// <param name="name">The name used to identify the object</param>
		/// <param name="obj">The object</param>
		/// <param name="tag">The data storage tab</param>
		public void AddObject(object obj, DataStorageTag tag, string name) {
			BinaryFormatter bf = new BinaryFormatter();
			System.IO.FileInfo serFile = new System.IO.FileInfo(vaultDirectory + "\\" +
				name + extension);
			System.IO.FileStream fs = serFile.Open(System.IO.FileMode.Create, 
				System.IO.FileAccess.Write, System.IO.FileShare.None);

			bf.Serialize(fs, tag);
			bf.Serialize(fs, obj);
			fs.Flush();
			fs.Close();
		}
    
		/// <summary>
		/// Add an object to be managed by the manager, passing in the object and the
		/// name.  The data storage tag is the default data storage tag.
		/// </summary>
		/// <param name="name">The name of the object</param>
		/// <param name="obj">The object to maange</param>
		public void AddObject(object obj, string name) {
			DataStorageTag tag = GetStorageTag(name);
			AddObject(obj, tag, name);
		}
    
		/// <summary>
		/// Cleans out the vault for the given file extension.
		/// </summary>
		/// <param name="extension">The file extension to remove</param>
		public void CleanVault(string extension) {
			ArrayList files = ListFilenames();
			foreach(string fname in files) {
				RemoveFile(fname);
			}
		}
	    
		/// <summary>
		/// Return an object from the vault.
		/// </summary>
		/// <param name="name">The name of the object</param>
		/// <returns>The object</returns>
		public object GetObject(string name) {
			System.IO.FileInfo objFile = new System.IO.FileInfo(vaultDirectory + "\\" +
				name + extension);
			System.IO.FileStream fs = objFile.Open(System.IO.FileMode.Open, 
				System.IO.FileAccess.Read, System.IO.FileShare.Read);
			BinaryFormatter bf = new BinaryFormatter();
			object dst = bf.Deserialize(fs);
			object result = bf.Deserialize(fs);
			fs.Close();
			return result;
		}
	    
		/// <summary>
		/// Returns the data storage tag for this object.
		/// </summary>
		/// <param name="name">The name of the object</param>
		/// <returns>The data storage tag</returns>
		public DataStorageTag GetStorageTag(string name) {
			System.IO.FileInfo objFile = new System.IO.FileInfo(vaultDirectory + "\\" +
				name + extension);
			System.IO.FileStream fs = objFile.Open(System.IO.FileMode.Open, 
				System.IO.FileAccess.Read, System.IO.FileShare.Read);
			BinaryFormatter bf = new BinaryFormatter();
			object result = bf.Deserialize(fs);
			fs.Close();
			return (DataStorageTag)result;
		}
	    
		/// <summary>
		/// Returns the vault directory.
		/// </summary>
		public string VaultDirectory {
			get {
				return vaultDirectory;
			}
			set {
				vaultDirectory = value;
			}
		}
	    
		/// <summary>
		/// Lists the names of the stored objects.
		/// </summary>
		/// <returns>The list of stored objects</returns>
		public ArrayList ListFilenames() {
			ArrayList result = null;

			System.IO.DirectoryInfo dirInfo = new System.IO.DirectoryInfo(vaultDirectory);
			System.IO.FileInfo[] files = dirInfo.GetFiles("*" + extension);

			result = new ArrayList();
			foreach(System.IO.FileInfo finfo in files) {
				result.Add(finfo.Name.Substring(0, finfo.Name.IndexOf(extension)));
			}
			return result;
		}
	    
		/// <summary>
		/// Removes the object from the vault.
		/// </summary>
		/// <param name="name">The name of the object</param>
		public void RemoveFile(string name) {
			System.IO.FileInfo finfo = new System.IO.FileInfo(vaultDirectory + "\\" +
				name + extension);
			if(finfo.Exists) {
				finfo.Delete();
			}
		}
	    
		/// <summary>
		/// Update the object with the given name.
		/// </summary>
		/// <param name="data">The object data</param>
		/// <param name="name">The name of the object to update</param>
		public void UpdateFile(string name, object data) {
			DataStorageTag tag = GetStorageTag(name);
			tag["Last Update"] = DateTime.Now;
			UpdateFile(data, tag, name);
		}
	    
		/// <summary>
		/// Update the file with the given name, with new data storage tag and new data.
		/// </summary>
		/// <param name="data">The object data</param>
		/// <param name="name">The name of the data object</param>
		/// <param name="tag">The storage </param>
		public void UpdateFile(object data, DataStorageTag tag, string name) {
			System.IO.FileInfo dataFile = new System.IO.FileInfo(vaultDirectory + "\\" +
				name + extension);
	        
			if(dataFile.Exists) {
				RemoveFile(name);
				AddObject(data, tag, name);
			}
		}
	}
}
