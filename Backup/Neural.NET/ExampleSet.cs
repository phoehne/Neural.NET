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
	/// An example set is a collection of examples, which can be subdivided,
	/// shuffled and manipulated.
	/// </summary>
	[Serializable]
	public class ExampleSet : System.Collections.IEnumerable
	{
        ArrayList examples = new ArrayList();
		System.Collections.IEnumerator exampleIterator = null;

		/// <summary>
		/// Default constructor.
		/// </summary>
		public ExampleSet()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		/// <summary>
		/// Add a new example to the set.
		/// </summary>
		/// <param name="example">A new example</param>
		public void AddExample(Example example) 
		{
			examples.Add(example);
		}

		/// <summary>
		/// Move to the next example in the example set.  Returns null
		/// if the set is empty, and will wrap around when it gets to the
		/// end of the set.
		/// </summary>
		/// <returns>The next example.</returns>
		public Example NextExample() 
		{
			if(examples.Count == 0) 
			{ 
				return null; 
			}

			if((exampleIterator == null) || (!exampleIterator.MoveNext()))
			{
				exampleIterator = examples.GetEnumerator();
				exampleIterator.MoveNext();
			}
			return (Example)exampleIterator.Current;
		}

		/// <summary>
		/// Indexer to provide access to the examples in this set.
		/// </summary>
		public Example this[int index]
		{
			get
			{
				return (Example)examples[index];
			}
		}

		/// <summary>
		/// Returns the number of examples in the example set.
		/// </summary>
		public int Count
		{
			get
			{
				return examples.Count;
			}
		}

		/// <summary>
		/// Constructs and example set from data read in as key, value pairs.  takes an
		/// array of input keys and ouptut keys to divide the raw data into a set of 
		/// inputs and expected values.
		/// </summary>
		/// <param name="data">A list of name value pairs of raw data</param>
		/// <param name="inputs">A list of names of input values</param>
		/// <param name="outputs">A list of names of output values</param>
		/// <returns>A constructed example set</returns>
		public static ExampleSet MakeExamples(List<Dictionary<String, double> > data, List<String> inputs, List<String> outputs) 
		{
			ExampleSet result = new ExampleSet();

			foreach(Dictionary<String, double> datum in data) 
			{
				Example ex = new Example();
				foreach(String name in inputs) 
				{
					ex.Inputs[name] = datum[name];
				}

				foreach(String name in outputs) 
				{
					ex.Expected[name] = datum[name];
				}
				result.AddExample(ex);
			}
			return result;
		}

		/// <summary>
		/// Checks to see the sizes are less than or equal to the example size.
		/// (Used when subidividing the example set.
		/// </summary>
		/// <param name="sizes">The sizes to segment the data</param>
		private void CheckSizeLimits(int[] sizes) 
		{
			int total = 0;
			foreach(int x in sizes) 
			{
				total += x;
			}
			if(total > Count) 
			{
				throw new ExampleSetException("Total number sizes exceeds example count.");
			}
		}

		/// <summary>
		/// Divide the example set into a number of smaller example sets, with 
		/// the sizes given in an array of values.
		/// </summary>
		/// <param name="sizes">The sizes to divide the result sets</param>
		/// <returns>A shuffled subsets of the original example set</returns>
		public ExampleSet[] DivideRandom(int[] sizes) 
		{
			CheckSizeLimits(sizes);
			ExampleSet[] result = new ExampleSet[sizes.Length];
			ArrayList indexes = new ArrayList();
			Random r = new Random();

			for(int i = 0; i < Count; i++) 
			{
				indexes.Add(i);
			}

			for(int sizeIdx = 0; sizeIdx < sizes.Length; sizeIdx++)  
			{
				ExampleSet xset = new ExampleSet();
				
				for(int i = 0; i < sizes[sizeIdx]; i++) 
				{
					int randIdx = (int)indexes[r.Next(indexes.Count)];
					Example ex = (Example)examples[randIdx];
					indexes.Remove(randIdx);
					xset.AddExample(ex);
				}

				result[sizeIdx] = xset;
			}
			return result;
		}

		/// <summary>
		/// Checkes the size limits for percentage sizes and throws an exception 
		/// when necessary.
		/// </summary>
		/// <param name="pct">The percentages.</param>
		private void CheckSizeLimits(double[] pct) 
		{
			double total = 0.0;
			foreach(double val in pct) 
			{
				total += val;
			}

			if(total > 1.0) 
			{
				throw new ExampleSetException("Total percentage sizes > 100%");
			}
		}

		/// <summary>
		/// Divide two example sets randomly, using sizes expressed as a percentage of the total size.
		/// The sum total of all the percentages must be less than or equal to 1.0.
		/// </summary>
		/// <param name="pctSizes">An array of percentages</param>
		/// <returns>The example sets with the percent sizes</returns>
		public ExampleSet[] DivideRandom(double[] pctSizes) 
		{
			CheckSizeLimits(pctSizes);
			int[] sizes = new int[pctSizes.Length];

			for(int i = 0; i < pctSizes.Length; i++) 
			{
				sizes[i] = (int)Math.Floor(pctSizes[i] * (double)(examples.Count));
			}

			return DivideRandom(sizes);
		}

		/// <summary>
		/// Divide an example set linearly (by simply iterating), with the given number of elements.
		/// The total number of elements for each returned sub-example must be less than or
		/// equal to the number of elements in the original example set.
		/// </summary>
		/// <param name="sizes">The sizes of the sub-sets</param>
		/// <returns>A group of example subsets</returns>
		public ExampleSet[] DivideLinear(int[] sizes) 
		{
			CheckSizeLimits(sizes);
			ExampleSet[] result = new ExampleSet[sizes.Length];

			int startAt = 0;

			for(int sizeIdx = 0; sizeIdx < sizes.Length; sizeIdx++) 
			{
				ExampleSet xset = new ExampleSet();
				for(int i = 0; i < sizes[sizeIdx]; i++) 
				{
					xset.AddExample((Example)(examples[i + startAt]));
				}
				startAt += sizes[sizeIdx];
				result[sizeIdx] = xset;
			}
			return result;
		}

		/// <summary>
		/// Divide the example sets linearly using the given percentage sizes.  The total sum
		/// of the percentages must be less than or equal to 1.0.
		/// </summary>
		/// <param name="pctSizes">The relative sizes of the subsets</param>
		/// <returns>The divided subsets</returns>
		public ExampleSet[] DivideLinear(double[] pctSizes) 
		{
			CheckSizeLimits(pctSizes);
			int[] sizes = new int[pctSizes.Length];

			for(int i = 0; i < sizes.Length; i++) 
			{
				sizes[i] = (int)Math.Floor(pctSizes[i] * (double)(examples.Count));
			}

			return DivideLinear(sizes);
		}

		/// <summary>
		/// Randomize the order of the example set.
		/// </summary>
		/// <returns>A randomized copy of this set.</returns>
		public ExampleSet Shuffle() 
		{
			int[] sizes = new int[1];
			sizes[0] = examples.Count;

			return DivideRandom(sizes)[0];
		}
		#region IEnumerable Members

		/// <summary>
		/// Return the enumerator to provide foreach support.
		/// </summary>
		/// <returns>The examples enumerator</returns>
		public IEnumerator GetEnumerator() {
			return examples.GetEnumerator();
		}

		#endregion
	}
}
