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
using Neural;
using Neural.Managers;

namespace Neural
{
	/// <summary>
	/// Summary description for Pocket.
	/// </summary>
	public class Pocket
	{
		string name;
		Hashtable managers = new Hashtable();
    
		/** Creates a new instance of Pocket */
		public Pocket(string name) 
		{
			this.name = name;
		}
    
		/// <summary>
		/// Adds a new error to monitor in the pocket.
		/// </summary>
		/// <param name="error">The name of the error</param>
		public void AddError(string error) 
		{
			if(!managers.ContainsKey(error)) 
			{
				managers[error] = new PocketManager(name, error);
			}
		}
    
		///
		/// Closes named error, cleaning up the pocket.
		///
		/// @param error The name of the error.
		///
		public void CloseError(string error) 
		{
			if(managers.ContainsKey(error)) 
			{
				PocketManager pm = (PocketManager)managers[error];
				pm.Cleanup();
				managers.Remove(error);
			}
		}
    
		///
		/// Returns the contents of the pocket or null if there is no network in
		/// the pocket.
		/// @param error The name of the error.
		/// @return The network.
		///
		public Network GetContents(string error) 
		{
			Network result = null;
			if(managers.ContainsKey(error)) 
			{
				PocketManager pm = (PocketManager)managers[error];
				result = pm.GetNetwork();
			} 
			return result;
		}
    

    
		///
		/// Return the epoch the pocket was updated.  A -1 is returned if there is
		/// no network in the pocket.
		/// @param error The name of the error.
		/// @return The epoch number.
		///
		public int GetLastUpdateEpoch(string error) 
		{
			int result = -1;
			PocketManager manager = (PocketManager)managers[error];
			result = manager.GetLastUpdateEpoch(); 
			return result;
		}
    
		///
		/// Returns the last update error for the network in the pocket or the 
		/// MAX_VALUE of double if there is no network in the pocket.
		/// @param error The name of the error.
		/// @return The error.
		///
		public double GetLastUpdateError(string error) 
		{
			double result = Double.MaxValue;
			PocketManager manager = (PocketManager)managers[error];
			result = manager.GetLastUpdateError();
			return result;
		}
    
		///
		/// Returns the names of the errors that are managed the this pocket.
		/// @return The list of error names.
		///
		public ArrayList GetErrors() 
		{
			return new ArrayList(managers.Keys);
		}
    
		///
		/// Save the network to the pocket with the named error calculator.
		/// @param error The name of the error.
		/// @param network The network to save.
		/// @param trainer The trainer training the network.
		///
		public void SaveNetwork(string error, Network network, Trainer trainer) 
		{
			PocketManager manager = (PocketManager)managers[error];
			if(trainer.ErrorManager.GetError(error) < manager.GetLastUpdateError())  
			{
				manager.SaveNetwork(network, trainer);
			}
		}
	}
}
