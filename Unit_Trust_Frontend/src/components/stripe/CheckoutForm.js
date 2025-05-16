import React, { useState } from 'react';
import { useStripe, useElements, PaymentElement } from '@stripe/react-stripe-js';
import { useNavigate } from 'react-router-dom'; // Importing useNavigate

const CheckoutForm = ({ amount, campaignId, onClose }) => {
  const stripe = useStripe();
  const elements = useElements();
  const [loading, setLoading] = useState(false);
  const [errorMessage, setErrorMessage] = useState(null);
  const navigate = useNavigate(); // Hook for navigation

  const handleError = (error) => {
    setLoading(false);
    setErrorMessage(error.message);
  };

  const handleSubmit = async (event) => {
    event.preventDefault();

    if (!stripe || !elements || loading) return;  // Prevent multiple submits

    setLoading(true);

    const { error, paymentIntent } = await stripe.confirmPayment({
      elements,
      confirmParams: {
        return_url: window.location.href,
      },
      redirect: "if_required"
    });

    if (error) {
      handleError(error);
    } else if (paymentIntent && paymentIntent.status === 'succeeded') {
      // Save payment to backend
      await fetch('https://localhost:7182/api/payment/save', {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json'
        },
        body: JSON.stringify({
          campaignId,
          amount: amount,
          status: paymentIntent.status,
          transactionDate: new Date().toISOString()
        })
      });

      alert('Payment successful!');
      navigate("/CampaignCard"); // Navigate to InvestorDashboard after success
      if (onClose) onClose();
    } else {
      alert('Payment is incomplete. Please try again.');
    }

    setLoading(false);
  };

  return (
    <form onSubmit={handleSubmit}>
      <p>You are donating MWK {amount.toLocaleString()}</p>  {/* Confirmation message */}
      <PaymentElement />
      <button type="submit" disabled={loading || !stripe}>
        {loading ? 'Processing…' : 'Pay Now'}
      </button>
      {errorMessage && <div className="error-message">{errorMessage}</div>}
    </form>
  );
};

export default CheckoutForm;
