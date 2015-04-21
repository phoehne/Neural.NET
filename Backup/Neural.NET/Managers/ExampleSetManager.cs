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
	/// Manages serialization of example sets.
	/// </summary>
	public class ExampleSetManager {

		static ExampleSetManager manager = null;
		GenericManager myManager;
    
		/// <summary>
		/// Creates a new instance of ExampleSetManager 
		/// </summary> 
		private ExampleSetManager() {
			myManager = new GenericManager();
			myManager.Extension = ".esdata";
		}
    
		/// <summary>
		/// Used to obtain a reference to the example set manager, a singleton in this
		/// system.
		/// </summary>
		/// <returns>The manager</returns>
		public static ExampleSetManager GetManager() {
			if(manager == null) {
				manager = new ExampleSetManager();
			}
			return manager;
		}
    
		/// <summary>
		/// Sets the directory for the example set vault.
		/// </summary>
		public string VaultDirectory {
			get {
				return myManager.VaultDirectory;
			}
			set {
				myManager.VaultDirectory = value;
			}
		}
    
		/// <summary>
		/// Lists the example sets in this vault.
		/// </summary>
		/// <returns>The list of example sets stored in this vault</returns>
		public ArrayList ListDataFiles() {
			return myManager.ListFilenames();
		}
    
		/// <summary>
		/// Adds a new example set to the vault with the given name and the given
		/// storage tag.
		/// </summary>
		/// <param name="eset">The example set to store</param>
		/// <param name="name">The name of the example set</param>
		/// <param name="tag">The Data Storage Tag for the example set</param>
		public void AddDataFile(ExampleSet eset, DataStorageTag tag, String name) {
			myManager.AddObject(eset, tag, name);
		}
    
		/// <summary>
		/// Adds a new example set to the vault.  Calls the <code>addDataFile</code>
		/// method with a new empty Data Storage Tag.
		/// </summary>
		/// <param name="eset">The example set to store</param>
		/// <param name="name">The name of the example set</param>
		public void AddDataFile(ExampleSet eset, String name) {
			AddDataFile(eset, new DataStorageTag(), name);
		}
    
		/// <summary>
		/// Updates the given data file and data storage tag.
		/// </summary>
		/// <param name="eset">The data to update</param>
		/// <param name="name">The name of the data set</param>
		/// <param name="tag">The data storage tag for the data</param>
		public void UpdateDataFile(ExampleSet eset, DataStorageTag tag, String name) {
			myManager.UpdateFile(eset, tag, name);
		}

    
		/// <summary>
		/// Calls update data file but does not update the data storage tag.
		/// </summary>
		/// <param name="eset">The example set to update</param>
		/// <param name="name">The name of the example set</param>
		public void updateDataFile(ExampleSet eset, String name) {
			myManager.UpdateFile(name, eset);
		}
    
		/// <summary>
		/// Returns the data storage tage associates with a given example set.
		/// </summary>
		/// <param name="name">name The name of the example set</param>
		/// <returns>The associated data storage tag</returns>
		public DataStorageTag getStorageTag(string name) {
			return myManager.GetStorageTag(name);
		} 

		/// <summary>
		/// Returns the example set for the given name.
		/// </summary>
		/// <param name="name">The name of the example set</param>
		/// <returns>The example set</returns>
		public ExampleSet GetData(string name) {
			return (ExampleSet)myManager.GetObject(name);
		}
    
		/// <summary>
		/// Remove the data file from the vault.
		/// </summary>
		/// <param name="name">The name of the data file</param>
		public void RemoveDataFile(string name) {
			myManager.RemoveFile(name);
		}
    
		/// <summary>
		/// Cleans out the example sets from the vault.
		/// </summary>
		public void CleanVault() {
			myManager.CleanVault(".esdata");
		}
	}
}
