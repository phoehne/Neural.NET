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

namespace Neural.Managers
{
	/// <summary>
	/// Stores and manages collections of saved data readers.
	/// </summary>
	public class DataReaderManager
	{
		static DataReaderManager dataReaderManager;
		GenericManager myManager;
    
		/// <summary>
		/// Creates a new instance of DataReaderManager
		/// </summary>
		private DataReaderManager() 
		{
			myManager = new GenericManager();
			myManager.VaultDirectory = ".";
			myManager.Extension = ".reader";
		}
    
		/// <summary>
		/// Returns an instance of the data reader manager.
		/// </summary>
		/// <returns>The data reader manager</returns>
		public static DataReaderManager getManager() 
		{
			if(dataReaderManager == null) 
			{
				dataReaderManager = new DataReaderManager();
			}
			return dataReaderManager;
		}
    
		/// <summary>
		/// Add a new data reader to the vault.
		/// </summary>
		/// <param name="name">The name of the network</param>
		/// <param name="reader">The data reader</param>
		/// <param name="tag">The storage tag for this network</param>
		public void AddNetwork(Neural.Data.DataReader reader, DataStorageTag tag, string name) 
		{
			myManager.AddObject(reader, tag, name);
		}
    
		/// <summary>
		/// Add a data reader to the vault, using the default data storage tag.
		/// </summary>
		/// <param name="name">The name of the network</param>
		/// <param name="reader">The data reader</param>
		public void AddNetwork(Neural.Data.DataReader reader, string name) 
		{
			myManager.AddObject(reader, name);
		}
    
		/// <summary>
		/// Cleans the vault of all data readers.
		/// </summary>
		public void CleanVault() 
		{
			myManager.CleanVault(".reader");
		}
    
		/// <summary>
		/// Returns the base directory for the vault.
		/// </summary>
		public String VaultDirectory 
		{
			get 
			{
				return myManager.VaultDirectory;
			}
			set 
			{
				myManager.VaultDirectory = value;
			}
		}
    
		/// <summary>
		/// Returns the named data reader from the vault.
		/// </summary>
		/// <param name="name">The name of the data reader</param>
		/// <returns>The data reader</returns>
		public Neural.Data.DataReader GetReader(string name) 
		{
			return (Neural.Data.DataReader)myManager.GetObject(name);
		}
    
		/// <summary>
		/// Returns the data storage tag for the named data reader.
		/// </summary>
		/// <param name="name">The name of the data reader</param>
		/// <returns>The data storage tag</returns>
		public DataStorageTag GetStorageTag(string name) 
		{
			return myManager.GetStorageTag(name);
		}
    
		/// <summary>
		/// Lists the data readers in the vault.
		/// </summary>
		/// <returns>A list of the data readers in the vault</returns>
		public ArrayList ListReaders() 
		{
			return myManager.ListFilenames();
		}
    
		/// <summary>
		/// Remove the data reader from the vault.
		/// </summary>
		/// <param name="name">The name of the data reader to remove</param>
		public void RemoveReader(string name) 
		{
			myManager.RemoveFile(name);
		}
    
		/// <summary>
		/// Update the given data reader without changing the data storage tag.
		/// </summary>
		/// <param name="name">The name of the data reader to update</param>
		/// <param name="reader">The data reader configuration</param>
		public void UpdateReader(Neural.Data.DataReader reader, string name) 
		{
			myManager.UpdateFile(name, reader);
		}
    
		/// <summary>
		/// Update the given data reader and the data storage tag.
		/// </summary>
		/// <param name="name">The name of the network</param>
		/// <param name="reader">The data reader configuration</param>
		/// <param name="tag">The storage tag to update</param>
		public void UpdateReader(Neural.Data.DataReader reader, DataStorageTag tag, string name) 
		{
			myManager.UpdateFile(reader, tag, name);
		}
	}
}
