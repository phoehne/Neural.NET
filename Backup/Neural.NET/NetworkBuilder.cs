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

namespace Neural
{
	/// <summary>
	/// Summary description for NetworkBuilder.
	/// </summary>
	public class NetworkBuilder
	{
		ArrayList layerSizes = new ArrayList();
		ArrayList inputNeurons = new ArrayList();
		ArrayList outputNeurons = new ArrayList();
		ActivationFactory activationFactory = new Neural.Activation.SigmoidActivationFactory();
    
		/**
		 * Return the sizes of the layers of the network to build
		 * @return The sizes of the network's layers.
		 */    
		public ArrayList Sizes
		{
			get 
			{
				return layerSizes;
			}
			set 
			{
				layerSizes = new ArrayList(value);
			}
		}
    
		/**
		 * Adds a hidden layer to build with the given size.
		 * @param size The size of the hidden layer.
		 */
		public void AddHiddenLayer(int size) 
		{
			layerSizes.Add(size);
		}
    
		/**
		 * Adds an input neuron to the builder.
		 * @param name The name of the input neuron.
		 */
		public void AddInputNeuron(string name) 
		{
			inputNeurons.Add(name);
		}
    
		/**
		 * Adds a list of input neurons to the network builder.
		 * @param neurons The list of neurons.
		 */
		public void AddInputNeurons(ArrayList neurons) 
		{
			inputNeurons.AddRange(neurons);
		}
    
		/**
		 * Adds an output neuron to the builder.
		 * @param name The name of the output neuron.
		 */
		public void AddOutputNeuron(string name) 
		{
			outputNeurons.Add(name);
		}
    
		/**
		 * Adds a list of output neurons to the builder.
		 *
		 * @param neurons The list of neurons.
		 */
		public void AddOutputNeurons(ArrayList neurons) 
		{
			outputNeurons.AddRange(neurons);
		}
    
		/**
		 * Sets the factory that will create activation functions, therefore
		 * building the network with the desired activation function.
		 * @param factory The activation factory.
		 */
		public ActivationFactory ActivationFactory 
		{
			set 
			{
				activationFactory = value;
			}
			get 
			{
				return activationFactory;
			}

		}
    
		private void ConnectHiddenLayerToInputs(Network network, int layerIdx, int size) 
		{
			for(int neuronIdx = 0; neuronIdx < size; neuronIdx++) 
			{
				string newNeuronName = "Hidden " + layerIdx + ", " + neuronIdx;
				foreach(string name in inputNeurons) {
					network.Connect(name, newNeuronName);
				}
			}
		}
    
		private void ConnectHiddenLayerToOutputs(Network network, int layerIdx, int size) 
		{
			for(int neuronIdx = 0; neuronIdx < size; neuronIdx++) 
			{
				String newNeuronName = "Hidden " + layerIdx + ", " + neuronIdx;
				foreach(string name in outputNeurons) {
					network.Connect(newNeuronName, name);
				}
			}
		}
    
		private void CreateInputNeurons(Network network)  
		{
			foreach(string name in inputNeurons) 
			{
				network.AddInputNeuron(new Neuron(name));
			}
		}
    
		private void CreateOutputNeurons(Network network) 
		{
			foreach(string name in outputNeurons) 
			{
				network.AddOutputNeuron(new Neuron(name));
			}
		}
    
		private void CreateInternalNeurons(Network network) 
		{
			for(int sizeIdx = 0; sizeIdx < layerSizes.Count; sizeIdx++) 
			{
				int hiddenLayerSize = (int)layerSizes[sizeIdx];
				for(int neuronIdx = 0; neuronIdx < hiddenLayerSize; neuronIdx++) 
				{
					string newNeuronName = "Hidden " + sizeIdx + ", " + neuronIdx;
					network.AddInternalNeuron(new Neuron(newNeuronName));
				}
			}
		}
    
		private void ConnectInternalLayers(Network network, int layerIdx, 
			int layerSize, int priorSize) 
		{
			for(int currNeuronIdx = 0; currNeuronIdx < layerSize; currNeuronIdx++) 
			{
				for(int priorNeuronIdx = 0; priorNeuronIdx < priorSize; priorNeuronIdx++) 
				{
					String currNeuronName  = "Hidden " + layerIdx       + ", " + currNeuronIdx;
					String priorNeuronName = "Hidden " + (layerIdx - 1) + ", " + priorNeuronIdx;
					network.Connect(priorNeuronName, currNeuronName);
				}
			}
		}
    
		/**
		* Build a network.  This method causes the network builder to build a fully
		* connected, feed-forward neural network.
		* @return A constructed network.
		*/    
		public virtual Network BuildNetwork() 
		{
			Network result = null;
        
			result = new Network();
			result.ActivationFactory = activationFactory;
			CreateInputNeurons(result);
			CreateOutputNeurons(result);
			CreateInternalNeurons(result);
        
			if(layerSizes.Count > 0) 
			{
				for(int i = 0; i < layerSizes.Count; i++) 
				{
					int size = (int)layerSizes[i];
                
					//
					// If this is the first hidden layer - connect it to the inputs.
					//
					if(i == 0) 
					{
						ConnectHiddenLayerToInputs(result, i, size);
					}
                
					//
					// If this is the last hidden layer - connect it to the outputs.
					// Note that if there is 1 hidden layer then we are done connecting
					// the layer.
					//
					if(i == (layerSizes.Count - 1)) 
					{
						ConnectHiddenLayerToOutputs(result, i, size);
					} 
                
					//
					// if we are not the first layer of hidden neurons, we need to
					// connect to the previous layer.  We only enter this if we have
					// more than one layer.
					//
					if((i != 0) && (i < layerSizes.Count)) 
					{
						ConnectInternalLayers(result, i, size, (int)layerSizes[i - 1]);
					}
				}
			}
        
			return result;
		}
	}
}
