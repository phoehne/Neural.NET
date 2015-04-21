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

namespace Neural.Data {
	/// <summary>
	/// Translates values into N-1 classes.  For example, three distinct values
	/// are translated into two output values (where { 0.0, 0.0 } is the third 
	/// class.
	/// </summary>
	public class NMinusOneClassTranslator : Translator {
		/// <summary>
		/// Default constructor.
		/// </summary>
		public NMinusOneClassTranslator() {
			//
			// TODO: Add constructor logic here
			//
		}

		/// <summary>
		/// Returns the output size of the translator.
		/// </summary>
		public override int OutputSize {
			get {
				return keys.Count - 1;
			}
		}

		/// <summary>
		/// The N-1 translator returns N-1 doubles for N possible strings.  For
		/// example, if the strings are { "car", "plane", "train", "boat" } then
		/// there are 4 possible strings of 3 values { 0.0, 0.0, 0.0 } for "car",
		/// { 1.0, 0.0, 0.0 } for "plane", { 0.0, 1.0, 0.0 } for "traine", and
		/// { 0.0, 0.0, 1.0 } for "boat".  If an unexpected string is entered,
		/// "rocket", for example, then an exception is thrown.
		/// </summary>
		/// <param name="values">The string to be translated</param>
		/// <returns>The array representing this string</returns>
		public override double[] Translate(string values) {
			double[] result = new double[keys.Count - 1];
			if(!keys.Contains(values)) {
				throw new TranslatorException("\"" + values + "\" is not defined in the set of applicable values.");
			}
        
			for(int i = 0; i < result.Length; i++) {
				result[i] = 0.0;
			}
			
			if(keys.IndexOf(values) > 0) {
				result[keys.IndexOf(values) - 1] = 1.0;
			}
			return result;
		}
	}
}
