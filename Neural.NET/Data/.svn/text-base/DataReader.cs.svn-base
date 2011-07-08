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

namespace Neural.Data {
	/// <summary>
	///
	/// <p>
	/// This is an abstract data reader.  A data reader will pull data from
	/// some source, like a file or JDBC connection, and return a data set.
	/// The input fields are optionally scaled or translated.  For example,
	/// a data file with the fields <br/>
	/// <code>30, 0.005, green</code><br/>
	/// <code>12, 0.006, blue</code><br/>
	/// <code>87, 0.002, red</code><br/>
	/// Could be translated to:<br/>
	/// <code>0.30, 0.5, 0.1, 0.9, 0.1</code><br/>
	/// <code>0.12, 0.6, 0.1, 0.1, 0.9</code><br/>
	/// <code>0.87, 0.2, 0.9, 0.1, 0.1</code><br/>
	/// <br/>
	/// The translators handle the changing of text input to numeric input and
	/// scalers adjust the range of the numeric input.  The raw input would first
	/// be translated to:<br/>
	/// <code>30.0, 0.005, 0.0, 1.0, 0.0</code><br/>
	/// <code>12.0, 0.006, 0.0, 0.0, 1.0</code><br/>
	/// <code>87.0, 0.002, 1.0, 0.0, 0.0</code><br/>
	/// <br/>
	/// Then it would be scaled to:<br/>
	/// <code>0.30, 0.5, 0.1, 0.9, 0.1</code><br/>
	/// <code>0.12, 0.6, 0.1, 0.1, 0.9</code><br/>
	/// <code>0.87, 0.2, 0.9, 0.1, 0.1</code><br/>
	/// </p>
	/// <p>
	/// Returning a raw list of values, however, would require the user to find
	/// a value remembering the position of the value.  For example wieght is the 
	/// 3rd value, and height is the 17th.  Instead, the result of reading data
	/// is returned as a list of maps.  The list preserves the order in which data 
	/// was read.  The rows in the list, however, are maps based on name-value 
	/// pairs.  The names are taken from the column definitions.
	/// </p>
	/// <p>
	/// The basic operation of the data reader is first to define input columns. 
	/// Any translations are defined at this point.  The columns are defined, and
	/// any scaling to be applied to raw input values.  The readValues method, 
	/// which is implemented in a concrete subclass, returns name value pairs where
	/// the name is taken from the column definition.
	/// </p>
	///
	/// </summary>
	public abstract class DataReader {

		/// <summary>
		/// Default constructor.
		/// </summary>
		public DataReader() {
			//
			// TODO: Add constructor logic here
			//
		}

		/// <summary>
		/// The fields for this data reader.
		/// </summary>
		protected ArrayList fields = new ArrayList();
		/// <summary>
		/// The columns for this data reader.
		/// </summary>
		protected ArrayList columns = new ArrayList();
		/// <summary>
		/// The fields indexed by name.
		/// </summary>
		protected Hashtable fieldByName = new Hashtable();
		/// <summary>
		/// The columsn indexed by name.
		/// </summary>
		protected Hashtable columnByName = new Hashtable();
    
		/// <summary>
		/// Define a new column in the output set with the given name and scaler.
		/// </summary>
		/// <param name="name">The column name</param>
		/// <param name="scaler">The scaler used to transform the column</param>
		public void DefineColumn(string name, Scaler scaler) {
			Column column = new Column();
			column.Scaler = scaler;
			column.Name = name;
			columns.Add(column);
			columnByName[name] = column;
		}
    
		/// <summary>
		/// Define a column in the output set, that does not scale.
		/// </summary>
		/// <param name="name">The column name</param>
		public void DefineColumn(string name) {
			Column column = new Column();
			column.Name = name;
			columns.Add(column);
			columnByName[name] = column;
		}
    
		/// <summary>
		/// Defines a real valued input field in the data set with the given
		/// name.
		/// </summary>
		/// <param name="name">The field name</param>
		public void DefineRealInputField(string name) {
			RealField field = new RealField();
			field.Name = name;
			fields.Add(field);
			fieldByName[name] = field;
		}
    
		/// <summary>
		/// Defines a translated input field with the given name.  Normally translated
		/// input fields are used for classifiers.  An example is translating a color
		/// to a set of K classes uses the KClassTranslator.
		/// </summary>
		/// <param name="name">The name of the input field</param>
		/// <param name="translator">The translator used to modify the value</param>
		public void DefineTranslatedInputField(string name, Translator translator) {
			TranslatedField field = new TranslatedField();
			field.Name = name;
			field.Translator = translator;
			fields.Add(field);
			fieldByName[name] = field;
		}
    
		/// <summary>
		/// Performs translation on a raw line.  When a line is read in from a data
		/// source it is parsed into an array of strings.  The array of strings
		/// is translated into an array of doubles.
		/// </summary>
		/// <param name="aLine">The array of strings</param>
		/// <returns>The doubles after translating the strings</returns>
		public double[] TranslateLine(string[] aLine) {
			if(aLine.Length != fields.Count) {
				throw new DataReadingException("Data reader expects: " + fields.Count + " values but " + aLine.Length + " items were provided");
			}
        
			double[] result = new double[columns.Count];
        
			int i = 0;
			foreach(Field field in fields) {
				try {
					double[] transValue = field.Process(aLine[i]);
					for(int j = 0; j < transValue.Length; j++) {
						result[i] = transValue[j];
						i++;
					}
				} 
				catch(TranslatorException te) {
					throw new DataReadingException("Error translating input", te);
				} 
				catch(Exception e) {
					throw new DataReadingException("Error reading input", e);
				}
			}
			return result;
		}
    
		/// <summary>
		/// Performs the scaling on a line translated into doubles.  After a line
		/// is read and translated into an array of doubles, it is scaled accordingly.
		/// The scaling is defined using the <c>defineColumn</c> method.
		/// </summary>
		/// <param name="rawLine">The raw double values read in</param>
		/// <returns>The scaled double values</returns>
		public double[] ScaleLine(double[] rawLine) {
			if(rawLine.Length != columns.Count) {
				throw new DataReadingException("The number of columns defined is " + columns.Count + " but there are " + rawLine.Length + " data elements.");
			}
			double[] result = new double[rawLine.Length];
			for(int i = 0; i < rawLine.Length; i++) {
				if(((Column)columns[i]).IsScaling) {
					result[i] = ((Column)columns[i]).Scaler.Scale(rawLine[i]);
				} 
				else {
					result[i] = rawLine[i];
				}
			}
			return result;
		}
    
		/// <summary>
		/// This is an abstract method that is implemented by a particular data reader,
		/// for example a JDBC data reader would read from a database.  The result is
		/// a list of name-value pairs, where the names are assigned by the column
		/// names.
		/// </summary>
		/// <returns>A list of name-value pairs</returns>
		public abstract List<Dictionary<String, double>> ReadValues();
    
		/// <summary>
		/// Assigns the name to values.  The data reader's output consists of name-value
		/// pairs.  This matches a value to a column name.
		/// </summary>
		/// <param name="scaledLine">The inputs after scaling</param>
		/// <returns>The collection of doubles with their names attached</returns>
		public Dictionary<String, double> NameValues(double[] scaledLine) {
            Dictionary<String, double> result = new Dictionary<String, double>();
        
			for(int i = 0; i < scaledLine.Length; i++) {
				result[((Column)columns[i]).Name] = scaledLine[i];
			}
			return result;
		}
	}
}
