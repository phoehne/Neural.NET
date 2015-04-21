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
using System.Collections.Generic;

using Neural;

namespace Neural.Calculators {
	/// <summary>
	/// Mean squared error calculator.
	/// </summary>
	public class MSEErrorCalculator : ErrorCalculator {
		double size;

		/// <summary>
		/// Default constructor.  Takes as an argument the number of elements
		/// used to calculate the mean.
		/// </summary>
		/// <param name="sz">The number of elements to average over</param>
		public MSEErrorCalculator(double sz) {
			size =sz;
		}
		#region ErrorCalculator Members

		/// <summary>
		/// Calculated the mean squared error contribution from these expected and
		/// actual values.  Note that we calculate the values, divided by the total
		/// number of elements, even though we are only looking at one element.  In
		/// the error manager, these are summed.  So, we return this element's contribution
		/// to the overall error.
		/// </summary>
		/// <param name="expected">The expected values</param>
		/// <param name="actual">The actual values</param>
		/// <returns>The contribution to Mean Squared Error</returns>
		public double CalculateError(IDictionary<String, double> expected, IDictionary<String, double> actual) {
			double result = 0.0;
        
			foreach(string key in actual.Keys) {
				result += Math.Pow(expected[key] - actual[key], 2.0)/2.0;
			}
			return result/size;
		}

		/// <summary>
		/// The default name for this error calcualtor.
		/// </summary>
		public string DefaultName {
			get {
				return "MSE";
			}
		}

		#endregion
	}
}
