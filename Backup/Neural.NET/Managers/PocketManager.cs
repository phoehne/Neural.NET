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

namespace Neural.Managers {
	/// <summary>
	/// The pocket manager manages a pocket of trained networks.
	/// </summary>
	public class PocketManager {
		string fileName = "pocket.pocket";
		GenericManager manager = null;
		bool saved = false;
		string error;
		string extension = ".pocket";
		double lastErrorVal = Double.MaxValue;
		int lastUpdateEpoch = -1;
    
		/// <summary>
		/// Creates a new instance of PocketManager
		/// </summary>
		/// <param name="error">The name of the error used for the pocket</param>
		/// <param name="pocketName">The name of the pocket</param>
		public PocketManager(string pocketName, string error) {
			fileName = pocketName + "." + error;
			this.error = error;
			manager = new GenericManager();
			manager.Extension = extension;
		}
    
    
		/// <summary>
		/// Saves the network to the pocket if it has a lower error than
		/// the existing version of the network.
		/// </summary>
		/// <param name="network">The network to add to the pocket</param>
		/// <param name="trainer">The trainer training the network</param>
		public void SaveNetwork(Network network, Trainer trainer) {
			if(!saved) {
				DataStorageTag tag = new DataStorageTag();
				tag.Description = "Pocket to manage the " + error + " error.";
				tag["last updated"] = trainer.EpochCount;
				tag["error value"] = trainer.ErrorManager.GetError(error);
				manager.AddObject(network, tag, fileName);
				saved = true;
			} 
			else {
				DataStorageTag tag = manager.GetStorageTag(fileName);
				tag["last updated"] = trainer.EpochCount;
				tag["error value"] = trainer.ErrorManager.GetError(error);
				manager.UpdateFile(network, tag, fileName);
			}
			lastUpdateEpoch = trainer.EpochCount;
			lastErrorVal = trainer.ErrorManager.GetError(error);
		}
    
		/// <summary>
		/// Clean up the pocket contents at the end of training.
		/// </summary>   
		public void Cleanup() {
			manager.RemoveFile(fileName);
		}
    
		/// <summary>
		/// Returns the network from the pocket.
		/// </summary>
		/// <returns>The network in pocket</returns>
		public Network GetNetwork() {
			return (Network)manager.GetObject(fileName);
		}
    
		/// <summary>
		/// Returns the epoch number when the pocket was last updated.
		/// </summary>
		/// <returns>The epoch number</returns>
		public int GetLastUpdateEpoch() {
			return lastUpdateEpoch;
		}
    
		/// <summary>
		/// Returns the error of the network in the pocket.
		/// </summary>
		/// <returns>The error</returns>
		public double GetLastUpdateError() {
			return lastErrorVal;
		}
	}
}
