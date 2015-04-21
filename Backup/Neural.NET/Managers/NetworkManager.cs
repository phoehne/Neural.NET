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
	/// Manages serialized, trained networks.
	/// </summary>
	public class NetworkManager {
		static NetworkManager networkManager;
		GenericManager myManager;
    
		/// <summary>
		/// Default constructor.
		/// </summary>
		private NetworkManager() {
			myManager = new GenericManager();
			myManager.VaultDirectory = ".";
			myManager.Extension = ".network";
		}
    
		/// <summary>
		/// Returns an instance of the network manager.
		/// </summary>
		/// <returns>The network manager</returns>
		public static NetworkManager GetManager() {
			if(networkManager == null) {
				networkManager = new NetworkManager();
			}
			return networkManager;
		}
    
		/// <summary>
		/// Add a new network to the vault.
		/// </summary>
		/// <param name="name">The name of the network</param>
		/// <param name="network">The network</param>
		/// <param name="tag">The storage tag for this network</param>
		public void AddNetwork(Network network, DataStorageTag tag, string name) {
			myManager.AddObject(network, tag, name);
		}
    
		/// <summary>
		/// Add a network to the vault, using the default data storage tag.
		/// </summary>
		/// <param name="name">The name of the network</param>
		/// <param name="network">The network</param>
		public void addNetwork(Network network, string name) {
			myManager.AddObject(network, name);
		}
    
		/// <summary>
		/// Cleans the vault of all networks.
		/// </summary>
		public void CleanVault() {
			myManager.CleanVault(".network");
		}
    
		/// <summary>
		/// Returns the base directory for the vault.
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
		/// Returns the named network from the vault.
		/// </summary>
		/// <param name="name">The name of the network</param>
		/// <returns>The network</returns>
		public Network getNetwork(string name) {
			return (Network)myManager.GetObject(name);
		}
    
		/// <summary>
		/// Returns the data storage tag for the named network.
		/// </summary>
		/// <param name="name">The name of the network</param>
		/// <returns>The data storage tag</returns>
		public DataStorageTag GetStorageTag(String name) {
			return myManager.GetStorageTag(name);
		}
    
		/// <summary>
		/// Lists the networks in the vault.
		/// </summary>
		/// <returns>A list of the networks in the vault</returns>
		public ArrayList ListNetworks() {
			return myManager.ListFilenames();
		}
    
		/// <summary>
		/// Remove the network from the vault.
		/// </summary>
		/// <param name="name">The name of the network to remove</param>
		public void RemoveNetwork(string name) {
			myManager.RemoveFile(name);
		}
    
    
		/// <summary>
		/// Update the given network without changing the data storage tag.
		/// </summary>
		/// <param name="name">The name of the network to update</param>
		/// <param name="network">network The network data</param>
		public void UpdateNetwork(Network network, string name) {
			myManager.UpdateFile(name, network);
		}
    
		/// <summary>
		/// Update the given network and the data storage tag.
		/// </summary>
		/// <param name="name">The name of the network</param>
		/// <param name="network">The network data</param>
		/// <param name="tag">The storage tag to update</param>
		public void UpdateNetwork(Network network, DataStorageTag tag, String name) {
			myManager.UpdateFile(network, tag, name);
		}
	}
}
