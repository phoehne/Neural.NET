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

namespace Neural
{
	/// <summary>
	/// Summary description for Evaluator.
	/// </summary>
	public class Evaluator
	{
		/**
		 * The error manager responsible for calculating the error
		 * values.
		 */    
		Dictionary<String, ErrorCalculator> errorList = new Dictionary<String, ErrorCalculator>();
		Dictionary<String, double> errorValues = new Dictionary<String, double>();
    
		/// <summary>
		/// Default constructor.
		/// </summary>
		public Evaluator() 
		{
			//this.manager = manager;
		}
    
		/// <summary>
		/// Adds a new error calculator to the evaluator.
		/// </summary>
		/// <param name="ec">The error calculator</param> 
		public void AddErrorCalculator(ErrorCalculator ec) 
		{
			AddErrorCalculator(ec, ec.DefaultName);
		}
    
		/// <summary>
		/// Add an error calculator with the given name, as opposed to the default
		/// name of the calculator.
		/// </summary>
		/// <param name="ec">The error calculator</param>
		/// <param name="name">The name of the error calculator</param>
		public void AddErrorCalculator(ErrorCalculator ec, string name) 
		{
			errorList[name] = ec;
			errorValues[name] = 0.0;
		}
    
		/// <summary>
		/// Returns the error names.
		/// </summary>
		/// <value>The error names</value>
		public ArrayList ErrorNames
		{
			get 
			{
				return new ArrayList(errorList.Keys);
			}
		}
    
		/// <summary>
		/// Return the error for the given name.
		/// </summary>
		/// <param name="name">The name of the error</param>
		/// <returns>The error</returns>
		public double GetError(string name) 
		{
			return (double)errorValues[name];
		}
    
		/// <summary>
		/// Evaluates the network, returning the evaluation of the error.
		/// </summary>
		/// <param name="examples">The examples to evaluate</param>
		/// <param name="network">The network to evalute</param>
		/// <returns>An error report for the valuation.</returns>
		public Dictionary<String, double> Evaluate(Network network, ExampleSet examples) 
		{
			ZeroErrors();
			for(int i = 0; i < examples.Count; i++) 
			{
				Example example = examples[i];
				Dictionary<String, double> results = network.Process(example.Inputs);
				CalculateErrors(example.Expected, results);
			}
			return errorValues;
		}
    
		private void ZeroErrors() 
		{
			ArrayList errorKeys = new ArrayList(errorValues.Keys);
			foreach(string name in errorKeys) {
				errorValues[name] = 0.0;
			}
		}
    
		private void Accumulate(string name, double val) 
		{
			double oldValue = (double)errorValues[name];
			errorValues[name] = oldValue + val;
		}
    
		private void CalculateErrors(Dictionary<String, double> expected, Dictionary<String, double> actual) 
		{
			foreach(string key in errorList.Keys) 
			{
				ErrorCalculator ec = (ErrorCalculator)errorList[key];
				double val = ec.CalculateError(expected, actual);
				Accumulate(key, val);
			}
		}
	}
}
