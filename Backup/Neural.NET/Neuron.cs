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
using Neural.Activation;

namespace Neural {
	/// <summary>
	/// A neuron, along with the axon, is the fundamental building block of the
	/// neural network.  Each neuron maintains information about inputs, outputs,
	/// and feedback through the neuron.  It is coupled to another neuron through
	/// an axon.  Each neuron has a unique id, and no two neurons in the same
	/// network can have the same id.
	/// </summary>
	[Serializable]
	public class Neuron {
        Dictionary<Guid, Axon> inputAxons = new Dictionary<Guid, Axon>();
        Dictionary<Guid, Axon> outputAxons = new Dictionary<Guid, Axon>();
        List<Guid> inputAxonIds = new List<Guid>();
        List<Guid> outputAxonIds = new List<Guid>();
        
        [NonSerialized]
        Dictionary<Guid, Guid> missingInput = null;
        [NonSerialized]
        Dictionary<Guid, Guid> missingFeedback = null;

	    double[] weights = new double[0];
		double[] inputs = new double[0];
		double[] feedback = new double[0];
		double outputValue;
		double rawValue;
		ActivationFunction activation = new SigmoidActivationFunction();
		string name;
		[NonSerialized] 
        NeuronTraining training;
    

		/// <summary>
		/// Returns the inputs for this neuron.
		/// </summary>
		public double[] Inputs {
			get {
				return inputs;
			}
		}

		/// <summary>
		/// Returns the name of this neuron.
		/// </summary>
		public string Name {
			get {
				return name;
			} 
			set {
				name = value;
			}
		}
    
		/// <summary>
		/// Default constructor.  Sets the default name to a GUID.
		/// </summary>
		public Neuron() {
			name = Guid.NewGuid().ToString();
		}
    
		/// <summary>
		/// Constructor - requires a name which needs to be unique for the
		/// network.
		/// </summary>
		/// <param name="name">The name of this neuro</param>
		public Neuron(string name) {
			this.name = name;
		}
    
		/// <summary>
		/// Add a new input axon to the neuron.  This will adjust the weights
		/// and inputs appropriately.  Since this is normally used in neural
		/// construction - it re-randomizes the weights.
		/// </summary>
		/// <param name="axon">The new input Axon</param>
		public void AddInputAxon(Axon axon) {
			inputAxonIds.Add(axon.Id);
			// inputAxonIndexes[axon.Id] = inputAxonIds.Count - 1;
			inputAxons[axon.Id] = axon;

			Random r = new Random();
        
			weights = new double[weights.Length + 1];
			for(int i = 0; i < weights.Length; i++) {
				weights[i] = r.NextDouble() * 2.0 - 1.0;
			}
        
			inputs = new double[inputs.Length + 1];
			for(int i = 0; i < weights.Length; i++) {
				inputs[i] = r.NextDouble();
			}
		}
    
		/// <summary>
		/// Sets the activation function used by this neuron.  The default
		/// activation function is the Sigmoid activation function.  The
		/// activation function must implement the ActivationFunction
		/// interface.
		/// </summary>
		public ActivationFunction ActivationFunction {
			get {
				return activation;
			}
			set {
				activation = value;
			}
		}
    
		/// <summary>
		/// Adds a new output axon to the neuron.  When the neuron fires,
		/// all the output axons are fired with the output value.
		/// </summary>
		/// <param name="axon">The new axon</param>
		public void AddOutputAxon(Axon axon) {
			outputAxonIds.Add(axon.Id);
			//outputAxonIndexes[axon.Id] = outputAxonIds.Count - 1;
			outputAxons[axon.Id] = axon;
			feedback = new double[outputAxonIds.Count];
		}
    
		/// <summary>
		/// This receives an input signal from the input axon.  It then
		/// initializes the set of missing inputs (if necessary) with the
		/// ids of the input axons.  If the axon is one of the axons that
		/// has not sent its signal, it is removed from the set of
		/// missing axons and the input value is stored.  If the size of the
		/// missing input set drops to zero, the output value is calculate,
		/// and the neuron is fired.
		/// </summary>
		/// <param name="firingAxon">The axon sending signal</param>
		/// <param name="val">The value firing into this neuron</param>
		public virtual void ReceiveSignal(Axon firingAxon, double val) {
			if(missingInput == null) {
				missingInput = new Dictionary<Guid, Guid>();
				foreach(Guid id in inputAxonIds) {
					missingInput[id] = id;
				}
				//missingInput.AddRange(inputAxonIds);
			}
        
			if(missingInput[firingAxon.Id] != null) {
				inputs[inputAxonIds.IndexOf(firingAxon.Id)] = val;
				//inputs[(int)(inputAxonIndexes[firingAxon.Id])] = val;
				missingInput.Remove(firingAxon.Id);
			}
        
			if(missingInput.Count == 0) {
				rawValue = DotProduct(inputs, Weights);
				outputValue = activation.Activation(rawValue);
				FireNeuron();
				missingInput = null;
			}
		}
    
		/// <summary>
		/// Calculates the dot-product of two arrays.
		/// </summary>
		/// <param name="p1">First array</param>
		/// <param name="p2">Second array</param>
		/// <returns>The dot-product</returns>
		protected double DotProduct(double[] p1, double[] p2) {
			double result = 0.0;
			for(int i = 0; i < p1.Length; i++) {
				result += p1[i] * p2[i];
			}
			return result;
		}
    
		/// <summary>
		/// If this neuron is an input neuron, then the output value is set
		/// by the neural network and the results are propagated to the
		/// output axons.
		/// </summary>
		/// <param name="val">The input value</param>
		public virtual void SetInput(double val) {
			outputValue = val;
			FireNeuron();
		}
    
		/// <summary>
		/// Iterates through the set of output axons, sending the output
		/// signal.
		/// </summary>
		protected virtual void FireNeuron() {
			foreach(Axon axon in outputAxons.Values) {
				axon.Propagate(outputValue);
			}
		}
    
		/// <summary>
		/// Returns the output value for this neuron.  Usually called for
		/// output neurons by the neural network.
		/// </summary>
		public double OutputValue {
			get {
				return outputValue;
			}
		}
    
		/// <summary>
		/// Set the error using the actual and expected values.  This 
		/// operates on the output nodes.  
		/// </summary>
		/// <param name="actual">The actual values obtained</param>
		/// <param name="expected">The expected values from an example</param>
		public virtual void SetError(double actual, double expected) {
			double delta = training.CalculateDelta(expected, actual, activation.Derivative(rawValue));
			training.CalculatePartials(inputs, delta);
			if(inputAxonIds.Count > 0) {
                foreach (var item in inputAxons)
                {
                    item.Value.Feedback(delta * Weights[inputAxonIds.IndexOf(item.Key)]);
                }
			}
		}
    
		/// <summary>
		/// Receives feedback from subsequent layer neurons.  As error
		/// information percolates back, the neuron will calculate its delta
		/// and partials.
		/// </summary>
		/// <param name="source">The axon sending the feedback</param>
		/// <param name="val">The feedback value</param>
		public virtual void ReceiveFeedback(double val, Axon source) {
			if(inputAxonIds.Count == 0) {
				return;
			}
        
			if(missingFeedback == null) {
				//missingFeedback = new ArrayList(outputAxonIds);
				missingFeedback = new Dictionary<Guid, Guid>();
				foreach(Guid id in outputAxonIds) {
					missingFeedback[id] = id;
				}
			}

			missingFeedback.Remove(source.Id);
			feedback[outputAxonIds.IndexOf(source.Id)] = val;
			// feedback[(int)(outputAxonIndexes[source.Id])] = val;
			if(missingFeedback.Count == 0) {
				double delta = training.CalculateDelta(feedback, activation.Derivative(rawValue));
				training.CalculatePartials(inputs, delta);
                foreach (var item in inputAxons)
                {
                    item.Value.Feedback(delta * Weights[inputAxonIds.IndexOf(item.Key)]);
                }
				
				missingFeedback = null;
			}
		}
    
		/// <summary>
		/// Set the neuron trainer for this neuron.
		/// </summary>
		/// <param name="trainer">The neuron trainer</param>
		public void SetNeuralTrainer(NeuronTraining trainer) {
			this.training = trainer;
			this.training.SetSize(Weights.Length);
		}
    
		/// <summary>
		/// Returns the affiliated neuron trainer.
		/// </summary>
		/// <returns>The neuron trainer attached to this neuron</returns>
		public NeuronTraining GetTrainer() {
			return training;
		}
    
		/// <summary>
		/// Adjust the weights.
		/// </summary>
		public virtual void AdjustWeights() {
			training.AdjustWeights(Weights);
			training.ClearPartials();
		}

		/// <summary>
		/// The weight vector for the neuron.
		/// </summary>
		public virtual double[] Weights {
			get {
				return weights;
			}
			set {
				if((weights == null) || (value == null) || (weights.Length != value.Length)) {
					System.Diagnostics.Trace.WriteLine("Weight values are null, values are null, or lengths do not match");
					throw new System.ArgumentException("Weight values are null, values are null, or lengths do not match");
				}
				for(int i = 0; i < weights.Length; i++) {
					weights[i] = value[i];
				}
			}
		}
	}
}
