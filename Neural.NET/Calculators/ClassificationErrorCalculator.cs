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
using System.Collections.Generic;
using Neural;

namespace Neural.Calculators {
	/// <summary>
	/// Calculates the error given the distance from the actual classification.
	/// Uses rounding to determine if the values are both in the same class 
	/// or both not in the same class (1.0 == 1.0) if there are in the same class
	/// or (0.0 == 0.0) if they are both not in the same class.  
	/// </summary>
	public class ClassificationErrorCalculator : ErrorCalculator {

		/// <summary>
		/// Default constructor.
		/// </summary>
		public ClassificationErrorCalculator() {
			//
			// TODO: Add constructor logic here
			//
		}
		#region ErrorCalculator Members

		/// <summary>
		/// Calculate the error based on the correct or incorrect classification.
		/// </summary>
		/// <param name="expected">The espected values</param>
		/// <param name="actual">The actual values</param>
		/// <returns>The classification error</returns>
		public double CalculateError(Dictionary<String, double> expected, Dictionary<String, double> actual) {
			double result = 0.0;
			string expectedClass = GetClass(expected);
			string actualClass = GetClass(actual);
			
			if(CountClasses(actual) > 1) {
				result = 1.0;
			}
			if(expectedClass != actualClass) {
				result = 1.0;
			}

			return result;
		}

		/// <summary>
		/// The default name of this error calculator.
		/// </summary>
		public string DefaultName {
			get {
				return "ClassError";
			}
		}

		#endregion

		/// <summary>
		/// Counts the number of indicated classifications in the
		/// result vector.
		/// </summary>
		/// <param name="resultSet">The result set</param>
		/// <returns>The number of indicated classes</returns>
		private int CountClasses(Dictionary<String, double> resultSet) {
			int result = 0;
            foreach (KeyValuePair<String, double> entry in resultSet)
            {
                if (Math.Round(entry.Value) == 1.0)
                {
                    result += 1;
                }
            }
			return result;
		}
    
		/// <summary>
		/// Returns the name of the max value from the error
		/// set.  If two things are in the same class, their max
		/// values should be the same.
		/// </summary>
		/// <param name="resultSet">The result set</param>
		/// <returns>The name of the maximum valued attribute</returns>
		private String GetClass(Dictionary<String, double> resultSet) {
			//double val = System.Double.NegativeInfinity;
			string result = "";
            foreach (KeyValuePair<String, double> entry in resultSet)
            {
                if (Math.Round(entry.Value) == 1.0)
                {
                    result = entry.Key;
                }
            }
			return result;
		}
	}
}
