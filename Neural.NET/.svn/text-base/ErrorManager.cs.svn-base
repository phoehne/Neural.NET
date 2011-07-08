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

namespace Neural {
	/// <summary>
	/// Manages collections of error calculators and their output.  Provides
	/// summaries of error information known as error reports.
	/// </summary>
	public class ErrorManager {

        Dictionary<String, ErrorCalculator> calculators = new Dictionary<string, ErrorCalculator>();
        Dictionary<String, double> errorReport = new Dictionary<String, double>();
		List<Dictionary<string, double>> errorHistory = new List<Dictionary<string, double>>();
    
		/// <summary>
		/// Default constructor
		/// </summary>
		public ErrorManager() {
		}
    
		/// <summary>
		/// Returns the number of registered calcualtors.
		/// </summary>  
		/// <value>The number of error calculators</value>
		public int Count {
			get {
				return calculators.Count;
			}
		}
    
		/// <summary>
		/// Adds a new calculator with the given name to the error
		/// manager.
		/// </summary>
		/// <param name="calculator">The calculator to add</param>
		/// <param name="name">The name of the calculator</param>
		public void AddCalculator(string name, ErrorCalculator calculator) {
			calculators[name] = calculator;
			errorReport[name] = 0.0;
		}
    
    
		/// <summary>
		/// Calculates the error using the available error
		/// calculators.  The results are saved into the error
		/// report property.
		/// </summary>
		/// <param name="actual">The actual network output</param>
		/// <param name="expected">The expected output</param>
		public void AccumulateError(Dictionary<String, double> expected, Dictionary<String, double> actual) {
			foreach(string errName in calculators.Keys) {
				double errVal = calculators[errName].CalculateError(expected, actual);
				Double accumError = (double)errorReport[errName];
				errorReport[errName] = errVal + accumError;
			}
		}
    
		/// <summary>
		/// Returns the error for a given error calculator.  This
		/// takes the error from the error report.  When training
		/// is complete this will always report 0, because the
		/// error report has been moved to the error history.
		/// </summary>
		/// <param name="name">The name of the error calculator</param>
		/// <returns>The error value.</returns>
		public double GetError(string name) {
			return (double)errorReport[name];
		}
    
		/// <summary>
		/// Clears the error report, moving it to the error History.
		/// </summary>
		public void ClearErrors() {
			errorHistory.Add(errorReport);
			errorReport = new Dictionary<string, double>();
        
			foreach(string errName in calculators.Keys) {
				errorReport[errName] = 0.0;
			}
		}
    
		/// <summary>
		/// Returns the error report, which contains the errors,
		/// organized by name.  This is moved to the error history
		/// at the end of each epoch during training.
		/// </summary>   
		public Dictionary<string, double> Report {
			get {
				return new Dictionary<string, double>(errorReport);
			}
		}
    
		/// <summary>
		/// Returns the size of the error history.  This may be
		/// a specious function, however, the error history may
		/// change to a sized window of k error reports, instead
		/// of all the error reports.
		/// </summary>
		/// <value>The sizeof the error history.</value>
		public int ErrorHistorySize {
			get {
				return errorHistory.Count;
			}
		}
    
		/// <summary>
		/// Returns the error history for a given error calculator.
		/// </summary>
		/// <param name="name">The name of the error history</param>
		/// <returns>The history of errors for the given calculator</returns>
		public ArrayList GetHistoricalErrors(string name) {
			ArrayList result = new ArrayList();
			foreach(Dictionary<string, double> theErrorMap in errorHistory) {
				result.Add(theErrorMap[name]);
			}
			return result;
		}
    
		/// <summary>
		/// Returns the last error report in history.  During training
		/// this is different from the current error report.  After
		/// training there is no current report, just history.
		/// </summary>
		/// <value>The last error report in the error history.</value>
		public Dictionary<string, double> LastErrors {
			get {
				Dictionary<string, double> result = null;
				if(errorHistory.Count > 0) {
					result = errorHistory[errorHistory.Count - 1];
				}
				return result;
			}
		}
    
		/// <summary>
		/// Returns the last error value for a given calculator.
		/// During training the current error value is in the
		/// error report, and the latest history is in the last
		/// history record.  After training there is only history.
		/// </summary>
		/// <param name="name">The name of the error calculator</param>
		/// <returns>The error value.</returns>
		public double GetLastError(string name) {
			Dictionary<string, double> report = LastErrors;
			if(report == null) {
				return Double.NaN;
			}
        
			return report[name];
		}
    
		/// <summary>
		/// Returns the list of calculator names.  These are all
		/// the calculators that have been registered with this
		/// error manager.
		/// </summary>
		/// <returns>The list of error calculators</returns>
		public ArrayList ListCalculators() {
			ArrayList result = new ArrayList(calculators.Keys);
			return result;
		}
    
		/// <summary>
		/// Returns an error calculator with a given name.
		/// </summary>
		/// <param name="name">The name of the error calculator</param>
		/// <returns>The error calcualtor</returns>    
		public ErrorCalculator GetCalculator(string name) {
			return (ErrorCalculator)calculators[name];
		}
	}
}
