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

namespace Neural {
	/// <summary>
	/// Neuron training assists the trainer at the level of the individual
	/// neuron.
	/// </summary>
	public class NeuronTraining {
		double delta = System.Double.NaN;
		double[] weightAdjustments;
		double[] priorWeightAdjustments;
		double[] partialDerivatives;
		double learningRate = 0.2;
		double momentum = 0.0;
		Trainer trainer = null;
    
		/// <summary>
		/// The trainer that owns this neuron trainer.  Neuron trainers are children
		/// of trainers, much the same way that neurons are the children of networks.
		/// </summary>
		public Trainer Trainer {
			get {
				return trainer;
			}
		}

		/// <summary>
		/// Return the weight adjustments.
		/// </summary>
		public double[] WeightAdjustments {
			get {
				return weightAdjustments;
			}
		}

		/// <summary>
		/// Read only copy of the prior weight adustments.
		/// </summary>
		public double[] PriorWeightAdjustments {
			get {
				return priorWeightAdjustments;
			}
		}

		/// <summary>
		/// Returns the partial derivatives.
		/// </summary>
		public double[] PartialDerivatives {
			get {
				return partialDerivatives;
			}
		}
    
		/// <summary>
		/// Sizes the arrays for the weight adjustmets, etc.  Normally,
		/// this is the size of the inputs to the attached neuron.
		/// </summary>
		/// <param name="sz">The size (in inputs) of the neuron to train</param>
		public void SetSize(int sz) {
			weightAdjustments = new double[sz];
			priorWeightAdjustments = new double[sz];
			partialDerivatives = new double[sz];
		}
    
		/// <summary>
		/// The learning rate used by this Neuron trainer.  Currently
		/// all neuron trainers attached to a given trainer use the same
		/// rate, but that may change in the future.
		/// </summary>
		public double LearningRate {
			get {
				return learningRate;
			}
			set {
				learningRate = value;
			}
		}

		/// <summary>
		/// The momentum coefficient used to calculate the weight change.
		/// </summary>
		public double Momentum {
			get {
				return momentum;
			}
			set {
				momentum = value;
			}
		}
  
		/// <summary>
		/// Default constructor.
		/// </summary>
		public NeuronTraining() {

		}
    
		/// <summary>
		/// Calculates the delta given the expected values, the actual values
		/// and the derivative of the activation value.
		/// </summary>
		/// <param name="actual">The actual value</param>
		/// <param name="derivative">The derivative of the activation</param>
		/// <param name="expected">The expected value</param>
		/// <returns>The delta value</returns>
		public double CalculateDelta(double expected, double actual, double derivative) {
			delta = -1.0 * (expected - actual) * derivative;
			return delta;
		}
    
		/// <summary>
		/// Calcuates the delta given the deltas of the subsequent layers and the
		/// weight connecting this neuron to the next neuron.  This is used on internal
		/// or "hidden" neurons to back propagate the error.  Normally this is defined
		/// as the Sum of the delta[i] * weight[i] for the i-th connection from this
		/// neuron to the next layer.  We pre-multiply the delta and weight when we
		/// back-propagate the feedback and therefore we need only sum it.
		/// </summary>
		/// <param name="derivative">The derivative of the activation</param>
		/// <param name="feedback">The feedback (delta and connecting weight)</param>
		/// <returns>The delta value</returns>
		public double CalculateDelta(double[] feedback, double derivative) {
			double raw = 0.0;
			for(int i = 0; i < feedback.Length; i++) {
				raw += feedback[i];
			}
			delta = raw * derivative;
			return delta;
		}
    
		/// <summary>
		/// Clears the partial derivatives, called after weights are adjusted.
		/// </summary>
		public void ClearPartials() {
			partialDerivatives = null;
		}
    
		/// <summary>
		/// Initializes the partials, if necessary.  Calculates the partials
		/// by summing them.  During an epoch the partial derivatives are cumulative.
		/// For on-line training, they are recalculated with ever input.
		/// </summary>
		/// <param name="delta">The calculated delta for this neuron</param>
		/// <param name="inputs">The inputs for this training example</param>
		public void CalculatePartials(double[] inputs, double delta) {
			if(partialDerivatives == null) {
				partialDerivatives = new double[inputs.Length];
				for(int i = 0; i < inputs.Length; i++) {
					partialDerivatives[i] = 0;
				}
			}
        
			for(int i = 0; i < inputs.Length; i++) {
				partialDerivatives[i] += delta * inputs[i];
			}
		}
    
		/// <summary>
		/// Return the partial derivatives.  This function seems pretty
		/// useless.
		/// </summary>
		public double[] Partials {
			get {
				return partialDerivatives;
			}
		}
    
		/// <summary>
		/// Updates the weight adjustments.  The weights are passed in only for
		/// the size.  Could be changed to an int parameter.  Could also pre-
		/// size the weight adjustment array size earlier.  Also stores off
		/// the prior weight adjustents
		/// </summary>
		public virtual void UpdateWeightAdjustments() {
			for(int i = 0; i < weightAdjustments.Length; i++) {
				weightAdjustments[i] += (-learningRate * partialDerivatives[i]
					+ momentum * priorWeightAdjustments[i]);
			}
		}
    
		/// <summary>
		/// Returns the weight updates - anther specious function.
		/// </summary>
		public double[] WeightUpdates {
			get {
				return weightAdjustments;
			}
		}
    
		/// <summary>
		/// Called to end the epoch.  Clears the partial derivatives.
		/// </summary>
		public void EndEpoch() {
			partialDerivatives = null;
        
			priorWeightAdjustments = (double[])weightAdjustments.Clone();
        
			for(int i = 0; i < weightAdjustments.Length; i++) {
				weightAdjustments[i] = 0.0;
			}
        
		}
    
		/// <summary>
		/// Called to adjust the weights.  Used the weight adjustments to update
		/// the weights passed in by reference.
		/// </summary>
		/// <param name="weights">The weights to adjust</param>
		public void AdjustWeights(double[] weights) {
			//updateWeightAdjustments(weights);
			for(int i = 0; i < weights.Length; i++) {
				weights[i] = weights[i] + weightAdjustments[i];
			}
		}
	}
}
