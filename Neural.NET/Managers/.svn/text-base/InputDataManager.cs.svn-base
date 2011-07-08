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

namespace Neural.Managers {
	/// <summary>
	/// Maintains a repository of input data.
	/// </summary>
	public class InputDataManager {
		static InputDataManager inputDataManager = null;
		GenericManager manager;
    
		/// <summary>
		/// Creates a new instance of InputDataManager.
		/// </summary>
		private InputDataManager() {
			manager = new GenericManager();
			manager.VaultDirectory = ".";
			manager.Extension = ".data";
		}
    
		/// <summary>
		/// Returns the input data manager.
		/// </summary>
		/// <returns>The input data manager</returns>
		public static InputDataManager GetManager() {
			if(inputDataManager == null) {
				inputDataManager = new InputDataManager();
			}
			return inputDataManager;
		}
    
		/// <summary>
		/// Adds data to the manager.
		/// </summary>
		/// <param name="data">The data to store</param>
		/// <param name="name">The name of the data file</param>
		/// <param name="tag">The data storage tag</param>
		public void AddInputData(ArrayList data, DataStorageTag tag, string name) {
			manager.AddObject(data, tag, name);
		}
    
		/// <summary>
		/// Adds data to the data manager.
		/// </summary>
		/// <param name="data">The data to manage</param>
		/// <param name="name">The name of the data file</param>
		public void AddInputData(ArrayList data, string name) {
			manager.AddObject(data, name);
		}
    
		/// <summary>
		/// Cleans the vault.
		/// </summary>   
		public void CleanVault() {
			manager.CleanVault(".data");
		}
    
		/// <summary>
		/// Returns the data set.
		/// </summary>
		/// <param name="name">The name of the data set</param>
		/// <returns>The data set</returns>
		public ArrayList getInputData(string name) {
			return (ArrayList)manager.GetObject(name);
		}
    
		/// <summary>
		/// Returns the data storage tag.
		/// </summary>
		/// <param name="name">The name of the data set</param>
		/// <returns>The data storage tag</returns>
		public DataStorageTag GetStorageTag(string name) {
			return (DataStorageTag)manager.GetStorageTag(name);
		}
    
		/// <summary>
		/// Returns the vault directory.
		/// </summary>
		public string VaultDirectory {
			get {
				return manager.VaultDirectory;
			}
			set {
				manager.VaultDirectory = value;
			}
		}
    
		/// <summary>
		/// Lists the data sets in the vault.
		/// </summary>
		/// <returns>The data sets in the vault</returns>
		public ArrayList ListDataSets() {
			return manager.ListFilenames();
		}
    
		/// <summary>
		/// Updates the given data set and the storage tag.
		/// </summary>
		/// <param name="data">The data set</param>
		/// <param name="name">The name of the data set</param>
		/// <param name="tag">The data storage tag</param>
		public void UpdateDataSet(ArrayList data, DataStorageTag tag, string name) {
			manager.UpdateFile(data, tag, name);
		}
    
		/// <summary>
		/// Saving the data set without modifying the storage tag.
		/// </summary>
		/// <param name="data">The data</param>
		/// <param name="name">The name of the data set</param>
		public void UpdateDataSet(ArrayList data, string name) {
			manager.UpdateFile(name, data);
		}
	}
}
