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

namespace Neural
{
	/// <summary>
	/// The axon that connects two neurons.
	/// </summary>
	/// <remarks>
	/// An axon connects two neurons and propagates the signal to 
	/// the next layer.  It also transports the feedback information
	/// during training.
	/// </remarks>
	[Serializable]
	public class Axon
	{
		private Guid id = Guid.NewGuid();
		private Neuron baseNeuron;
		private Neuron dendrite;
    
		/// <summary>
		/// Default constructor
		/// </summary>
		/// <remarks>
		///	Creates a new Axon with a new id.
		/// </remarks>
		public Axon() 
		{
			id = Guid.NewGuid();
		}
    
		/// <summary>
		/// Constructor with ID
		/// </summary>
		/// <remarks>
		/// The constructor builds a new Axon with the given ID.
		/// </remarks>
		/// <param name="id">The axon's id</param>
		public Axon(Guid id) 
		{
			this.id = id;
		}

		/// <summary>
		/// An axon id.
		/// </summary>
		/// <value>The id of this axon</value>
		public Guid Id 
		{
			get;
		}
    
		/// <summary>
		/// Axons have a dendrite, to which they pass values when they 
		/// fire and propagate back feedback to the base neuron.
		/// </summary>
		/// <value>The dendrite neuron for this axon.</value>
		public Neuron Dendrite 
		{
			get; set;
		}

		/// <summary>
		/// Returns the base of the Axon.  The base is the neuron that will fire
		/// into this axon, sending the signal to the dendrite.  When an
		/// axon carries a signal, it does so from the base to the dendrite.
		/// </summary>
		/// <value>The base of the axon or the source neuron.</value>
		public Neuron Base 
		{
			get; set;
		}
    
		/// <summary>
		/// Propagates the given signal to the dendrite neuron from the base.
		/// It notifies that dendrite neuron that it has fired, by passing
		/// itself into the <code>recieveSignal</code> method of the dendrite
		/// neuron.
		/// </summary>
		/// <value>The value to propagate.</value>
		public void Propagate(double val) 
		{
			dendrite.ReceiveSignal(this, val);
		}
    
		/// <summary>
		/// When training feedback is fired from the dendrite neuron back to the
		/// base.  This allows the base neuron to adjust its weight accordingly.
		/// For middle-tier neurons this consists of the delta * the connecting
		/// weight.
		/// </summary>
		/// <value>The feedback value.</value>
		public void Feedback(double val) 
		{
			Base.ReceiveFeedback(val, this);
		}
	}
}
