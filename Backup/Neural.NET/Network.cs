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
	/// Summary description for Network.
	/// </summary>
	[Serializable]
	public class Network {
		ArrayList axons = new ArrayList();

        Dictionary<string, Neuron> neurons = new Dictionary<string, Neuron>();
        Dictionary<string, Neuron> inputNeurons = new Dictionary<string, Neuron>();
        Dictionary<string, Neuron> outputNeurons = new Dictionary<string, Neuron>();
        
        ActivationFactory factory = new SigmoidActivationFactory();
		bool validateInputs = true;
		Neuron bias;

		/// <summary>
		/// Indexer to acces individual neurons.
		/// </summary>
		public Neuron this[string name] {
			get {
				return (Neuron)neurons[name];
			}
		}



		/// <summary>
		/// Returns the names of all the neurons.
		/// </summary>
		public ArrayList NueronNames {
			get {
				return new ArrayList(neurons.Keys);
			}
		}

		/// <summary>
		/// Shut off validating the input keys during production runs.
		/// Can save some time.
		/// </summary>
		public bool ValidateInputs {
			get {
				return validateInputs;
			}

			set {
				validateInputs = value;
			}
		}
    
		/// <summary>
		/// Create a new instance of a network.  By default a bias node
		/// is added to the network and automatically connected to any
		/// neurons added to the network (except input neurons).
		/// </summary>
		public Network() {
			bias = new Neuron("Bias neuron");
		}
    
		/// <summary>
		/// Set the activation factory which will create new activation functions.
		/// </summary>
		/// <value>factory The activation factory.</value>
		public ActivationFactory ActivationFactory {
			get {
				return factory;
			}
			set {
				factory = value;
			}
		}
    
		/// <summary>Add output neuron.</summary>
		/// <param name="neuron">The output neuron</param>
		public void AddOutputNeuron(Neuron neuron) {
			neuron.ActivationFunction = factory.MakeFunction();
			AddNeuron(neuron);
			outputNeurons[neuron.Name] = neuron;
			ConnectToBias(neuron);
		}
    
		/// <summary>
		/// Add input neuron.  Input neurons have their value "set" to a given value
		/// and then propagate that value to any neurons they are connected to.
		/// </summary>
		/// <param name="neuron">The new input neuron</param>
		public void AddInputNeuron(Neuron neuron) {
			AddNeuron(neuron);
			inputNeurons[neuron.Name] = neuron;
		}
    
		/// <summary>
		/// Adds a "hidden layer" neuron.  Hidden layers get their inputs from either
		/// input neurons or other hidden layers.  They then pass those values to the
		/// next output or hidden layer.
		/// </summary>
		/// <param name="neuron">The new neuron to add</param>
		public void AddInternalNeuron(Neuron neuron) {
			neuron.ActivationFunction = factory.MakeFunction();
			AddNeuron(neuron);
			ConnectToBias(neuron);
		}
    
		/// <summary>
		/// Connect a neuron to the bias neuron.
		/// </summary>
		/// <param name="neuron">The bias neuron</param>
		private void ConnectToBias(Neuron neuron) {
			Axon biasAxon = new Axon();
			biasAxon.Base = bias;
			biasAxon.Dendrite = neuron;
			bias.AddOutputAxon(biasAxon);
			neuron.AddInputAxon(biasAxon);
			axons.Add(biasAxon);
		}
    
		/// <summary>
		/// Connect two nuerons by their name.  This method is used to
		/// build up connections within the network.  The only connection established
		/// by default is the connection to the bias node.  This is the
		/// preferred method.
		/// </summary>
		/// <param name="baseNeuron">The name of the base neuron to connect</param>
		/// <param name="dendrite">The name of the dendrite neuron to connect</param>
		public void Connect(string baseNeuron, string dendrite) {
			Connect((Neuron)neurons[baseNeuron], (Neuron)neurons[dendrite]);
		}
    
		/// <summary>
		/// Connect two neurons.  This method is used to
		/// build up connections within the network.  The only connection established
		/// by default is the connection to the bias node.
		/// </summary>
		/// <param name="baseNeuron">A base neuron to connect</param>
		/// <param name="dendrite">A dendrite neuron to connect</param>
		public void Connect(Neuron baseNeuron, Neuron dendrite) {
			Axon axon = new Axon();
			axon.Base = baseNeuron;
			axon.Dendrite = dendrite;
			baseNeuron.AddOutputAxon(axon);
			dendrite.AddInputAxon(axon);
			axons.Add(axon);
		}
    
		/// <summary>
		/// Add a neuron to the network.  If a neuron with the same name has
		/// already been added to the network, a <code>DuplicateNeuronException</code>
		/// is thrown.  This need not be the same neuron, but all neurons that
		/// are part of this network must have unique names, with respect to
		/// the network.
		/// </summary>
		/// <param name="neuron">The neuron to add</param>
		private void AddNeuron(Neuron neuron) {
			if(neurons.ContainsKey(neuron.Name)) {
				throw new DuplicateNeuronException("Network already contains " + neuron.Name);
			}
			neurons[neuron.Name] = neuron;
		}
    
		/// <summary>
		/// Process a set of inputs and return a result.  The method checks to
		/// see that the names in the input map correspond to the names of the input
		/// neurons.  If they do not match, an <code>InvalidInputsException</code>
		/// is thrown.  It returns a map with the output values keyed on the names
		/// of the output neurons.
		/// </summary>
		/// <param name="inputs">The inputs to the network</param>
		/// <returns>The processed result</returns>
		public Dictionary<String, double> Process(Dictionary<String, double> inputs) {
            Dictionary<String, double> result = new Dictionary<String, double>();
			
			//if(inputs.Keys.Count != inputNeurons.Keys.Count) {
			//	throw new InvalidInputsException("Input value names do not match defined input neurons.");
			//}
			if(validateInputs) {
				foreach(string s in inputNeurons.Keys) {
					if(!inputs.ContainsKey(s)) {
						throw new InvalidInputsException(string.Format("Neuron '{0}' does not have a corresponding input value.", s));
					}
				}
			}

			bias.SetInput(1.0);
            foreach (KeyValuePair<String, Neuron> entry in inputNeurons)
            {
                entry.Value.SetInput(inputs[entry.Key]);
            }

            foreach (KeyValuePair<String, Neuron> entry in outputNeurons)
            {
                result[entry.Key] = entry.Value.OutputValue;
            }
			return result;
		}
    
		/// <summary>
		/// Returns the trainer attached to this network.  A trainer configures
		/// the network for training on a given method or algorithm.
		/// </summary>
		/// <param name="neuronId">The neuron trainer for the named neuron</param>
		/// <returns>The neuron trainer used by a given neuron</returns>
		public NeuronTraining getTrainer(string neuronId) {
			return ((Neuron)neurons[neuronId]).GetTrainer();
		}
		    
		/// <summary>
		/// During training a network is presented the actual and expected values
		/// for a given input.  Based on this, the network back-propagates the errors
		/// and by the derivative chaining rule, calculates the weight adjustments
		/// necessary to train the network, given the trainer.
		/// </summary>
		/// <param name="actual">The actual values obtained from executing the network</param>
		/// <param name="expected">The expected values from a training example</param>
		public void SetFeedback(Dictionary<String, double> actual, Dictionary<String, double> expected) {
            foreach (KeyValuePair<String, Neuron> entry in outputNeurons)
            {
                entry.Value.SetError((double)actual[entry.Value.Name], (double)expected[entry.Value.Name]);
            }
		}
    
		/// <summary>
		/// Attaches a trainer to the network and establishes any necessary structures
		/// for training.
		/// </summary>
		/// <param name="trainer">The trainer that will perform the training</param>
		public void BeginTraining(Trainer trainer) {
            foreach (KeyValuePair<String, Neuron> entry in neurons)
            {
                entry.Value.SetNeuralTrainer(trainer.GetTraining());
            }
		}
    
		/// <summary>
		/// Completes the training by removing training structures from the
		/// network.
		/// </summary>
		public void EndTraining() {
		        
		}
    
		/// <summary>
		/// Called during training to periodically adjust the weights
		/// on the network.
		/// </summary>
		public void AdjustWeights() {
            foreach (KeyValuePair<String, Neuron> entry in neurons)
            {
                entry.Value.AdjustWeights();
            }
		}
    
		/// <summary>
		/// Updates the weight adjustments for all the neurons in the network.  Note that
		/// this is not the same as updating the weights.  This just calculates the
		/// change in the weight.
		/// </summary>
		public void UpdateAdjustments () {
            foreach (KeyValuePair<String, Neuron> entry in neurons)
            {
                entry.Value.GetTrainer().UpdateWeightAdjustments();
            }
		}
	}
}
