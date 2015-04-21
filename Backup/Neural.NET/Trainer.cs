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
using Neural.Calculators;

namespace Neural
{
	/// <summary>
	/// A delegate method to be called when important things happen
	/// during the training epoch.
	/// </summary>
	public delegate void EpochEventFunction(TrainingEpochEvent evt);
	/// <summary>
	/// Summary description for Trainer.
	/// </summary>
	public class Trainer
	{
		ExampleSet trainingExamples;
		int epochSize;
		ErrorManager errorManager = new ErrorManager();
		string defaultErrorName;
		ArrayList neuronTrainers = new ArrayList();
		double learningRate = 0.20;
		double momentum = 0.0;
		bool terminating = false;
		ArrayList epochEventListeners = new ArrayList();
		int hardMax = 10000;
		int epochCount;

		/// <summary>
		/// The delegates to be called at the end of training.
		/// </summary>
		public EpochEventFunction EndEpochDelegates;
		/// <summary>
		/// Called when training is started.
		/// </summary>
		public EpochEventFunction StartTrainingDelegates;
		/// <summary>
		/// Called when each presentation is finished
		/// </summary>
		public EpochEventFunction EndPresentationDelegates;
		/// <summary>
		/// Used to notify after an update.
		/// </summary>
		public EpochEventFunction PostUpdateDelegates;

		/// <summary>
		/// If no EpochEventListeners are added to the network the hardMax
		/// limit is used to safely stop training.
		/// </summary>
		public int HardMax 
		{
			get 
			{
				return hardMax;
			}
			set 
			{
				hardMax = value;
			}
		}
    
		/// <summary>
		/// Set the default learning rate used for training.  The default
		/// is 0.20.  This is the learning rate passed to all the neuron
		/// trainers when they are created.
		/// </summary>
		public double LearningRate 
		{
			get 
			{
				return learningRate;
			}
			set 
			{
				learningRate = value;
				if(neuronTrainers.Count > 0) 
				{
					foreach(NeuronTraining trainer in neuronTrainers) 
					{
						trainer.LearningRate = learningRate;
					}
				}
			}
		}
    
		/// <summary>
		/// Returns the momentum used during training.  This is the
		/// default momentum passed to the neuron trainers when they are
		/// created.  The default is 0.00.
		/// </summary>
		public double Momentum 
		{
			get 
			{
				return momentum;
			}
			set 
			{
				momentum = value;
				if(neuronTrainers.Count > 0) 
				{
					foreach(NeuronTraining trainer in neuronTrainers) 
					{
						trainer.Momentum = momentum;
					}
				}
			}
		}
    
		/// <summary>
		/// The error calculator computes the error from the input values
		/// and the output values.  By default this is sum of squares.  This
		/// version of the addCalculator uses the default name for the 
		/// calculator.
		/// </summary>
		/// <param name="calculator">The calculator to set</param>
		public void AddCalculator(ErrorCalculator calculator) 
		{
			errorManager.AddCalculator(calculator.DefaultName, calculator);
		}
    
		/// <summary>
		/// The error calculator computes the error from the input values
		/// and the output values.  By default this is the sum of squares.
		/// </summary>
		/// <param name="name">The name of the error calculator</param>
		/// <param name="calculator">The calculator to set</param>
		public void AddCalculator(string name, ErrorCalculator calculator) 
		{
			errorManager.AddCalculator(name, calculator);
		}
    
		/// <summary>
		/// The epoch size is the number of examples to present during training
		/// before the error is saved and the weights are adjusted.  Normally,
		/// this is equal to the size of the training set.
		/// </summary>
		public int EpochSize 
		{
			get 
			{
				return epochSize;
			}
			set 
			{
				epochSize = value;
			}
		}
    
		/// <summary>
		/// The training examples are set of known inputs and their outputs
		/// used to train the network.  Normally the data is divided into
		/// a training set and a validation set.  The training set is used
		/// to train the network while the validation set is used to test
		/// the quality of the training.
		/// </summary>
		public ExampleSet TrainingExamples 
		{
			get 
			{
				return trainingExamples;
			}
			set 
			{
				trainingExamples = value;
				EpochSize = trainingExamples.Count;
			}
		}

		/// <summary>
		/// Default constructor.
		/// </summary>
		public Trainer() 
		{

		}
    
		/// <summary>
		///  <p>
		/// Called to train the network.  Iterates until the terminating flag is
		/// set.  If there are no training event listeners set then there is a
		/// hard termination limit of 10000 (by default) iterations.
		/// </p>
		/// <p>
		/// The training algorithm is simple.  It presents the training examples
		/// in order, up to epochSize before ending the epoch.  At that point the
		/// error for the epoch is calculated and the EndEpochEvent is sent to all
		/// the listeners.  The weights of the network are then adjusted.  Training
		/// stops when the hard limit is reached (given no listeners) or by a listener
		/// requesting a termination.
		/// </p>
		/// </summary>
		/// <param name="network"></param>
		public void Train(Network network)
		{
			Dictionary<String, double> results;
			terminating = false;
			network.BeginTraining(this);
			epochCount = 0;
        
			if(errorManager.Count == 0) 
			{
				errorManager.AddCalculator("MSE", new MSEErrorCalculator(trainingExamples.Count));
				defaultErrorName = "MSE";
			}

			if(StartTrainingDelegates != null) {
				StartTrainingDelegates(new TrainingEpochEvent(this, network));
			}
			while(!terminating) 
			{
				try 
				{
					for(int i = 0; i < epochSize; i++) 
					{
						Random r = new Random();
						Example example = trainingExamples[r.Next(0, trainingExamples.Count - 1)];
						results = network.Process(example.Inputs);
						errorManager.AccumulateError(example.Expected, results);
						network.SetFeedback(results, example.Expected);
						if(EndPresentationDelegates != null) {
							EndPresentationDelegates(new TrainingEpochEvent(this, network));
						}
					}
					epochCount++;

					if(EndEpochDelegates != null) {
						EndEpochDelegates(new TrainingEpochEvent(this, network));
					}

					UpdateNetwork(network);
					
					if(PostUpdateDelegates != null) {
						PostUpdateDelegates(new TrainingEpochEvent(this, network));
					}

					if((epochEventListeners.Count == 0) && (epochCount > hardMax)) 
					{
						terminating = true;
					}
				} catch(Exception e) 
				{
					Console.WriteLine(e.StackTrace);
					throw new TrainingException("Unable to train netowrk due to exception", e);
				}
			}
			network.EndTraining();
		}
    
		/// <summary>
		/// Causes the weight adjustments to be updated at all the neuron trainers.
		///  </summary>
		public void UpdateWeightAdjustments() 
		{
			foreach(NeuronTraining nt in neuronTrainers) {
				nt.UpdateWeightAdjustments();
			}
		}
    
		/// <summary>
		/// Called at the end of an epoch.  In this case it sends the end epoch
		/// message to all the neuron trainers.
		/// </summary>
		/// <param name="network">The network being trained</param>
		public void UpdateNetwork(Network network) 
		{
				//errors.add(new Double(error));
			UpdateWeightAdjustments();
			network.AdjustWeights();
			foreach(NeuronTraining nt in neuronTrainers) {
				nt.EndEpoch();
			}
			errorManager.ClearErrors();
		}
    
		/// <summary>
		/// Returns the default neuron training for this trainer.  It
		/// also sets the learning rate as appropriate.
		/// </summary>
		/// <returns>The neuron trainer used by this trainer.</returns>
		public NeuronTraining GetTraining() 
		{
			NeuronTraining result = new NeuronTraining();
			result.LearningRate = learningRate;
			result.Momentum = momentum;
			neuronTrainers.Add(result);
			return result;
		}
		    
    
		/// <summary>
		/// Used primarily by the epoch event listeners to request the
		/// neural network to stop training.
		/// </summary>
		public void RequestTermination() 
		{
			terminating = true;
		}
    
		/// <summary>
		/// Returns the epoch count so far
		/// </summary>
		public int EpochCount 
		{
			get 
			{
				return epochCount;
			}
		}
    
		/// <summary>
		/// Returns the error manager used by the trainer when
		/// calculating training error.
		/// </summary>
		public ErrorManager ErrorManager 
		{
			get 
			{
				return errorManager;
			}
		}
    
		/// <summary>
		/// Sets the default error name to examine.  If no error is
		/// specified, the default error name is used.
		/// </summary>
		public string DefaultErrorName 
		{
			set 
			{
				defaultErrorName = value;
			}
		}
	}
}
