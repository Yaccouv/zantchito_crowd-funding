import React, { useState } from 'react';
import { useLocation, useNavigate } from 'react-router-dom';
import Navbar from "../Navbar";
import Footer from "../Footer";
import { Elements } from '@stripe/react-stripe-js';
import { loadStripe } from '@stripe/stripe-js';
import CheckoutForm from '../stripe/CheckoutForm';
import Message from '../Message'; // Import the Message component

const stripePromise = loadStripe('pk_test_51KdYapJTwcjF35OzSybWMrrTTSBRmZJlFD1N0BWM22mPl8XXZ8HKF1tzJhKHpOamsxDkfTXu3B6ju2V3C2mDq1U200ejlyLZUR');

const PaymentModal = ({ amount, campaignId, onClose, onPaymentStatus }) => {
  const [clientSecret, setClientSecret] = useState(null);

  React.useEffect(() => {
    const createPaymentIntent = async () => {
      const response = await fetch("https://localhost:7182/api/payment/create-intent", {
        method: "POST",
        headers: {
          "Content-Type": "application/json"
        },
        body: JSON.stringify({
          amount: parseInt(amount * 100),
          campaignId
        })
      });

      const data = await response.json();
      setClientSecret(data.client_secret);
    };

    if (amount && campaignId) {
      createPaymentIntent();
    }
  }, [amount, campaignId]);

  return (
    <div className="payment-modal-overlay">
      <div className="payment-modal-content">
        <button onClick={onClose} className="close-button">X</button>
        {clientSecret && (
          <Elements stripe={stripePromise} options={{ clientSecret }}>
            <CheckoutForm 
              amount={amount} 
              campaignId={campaignId} 
              onClose={onClose} 
              onPaymentStatus={onPaymentStatus} // Pass status handler to CheckoutForm
            />
          </Elements>
        )}
      </div>
    </div>
  );
};

const Campaign = () => {
  const location = useLocation();
  const navigate = useNavigate();
  const { campaign } = location.state || {};
  const [amount, setAmount] = useState('');
  const [showPaymentModal, setShowPaymentModal] = useState(false);
  const [paymentMessage, setPaymentMessage] = useState(null); // State to store the payment status message

  const handleDonate = () => {
    if (!amount || isNaN(amount) || parseFloat(amount) <= 0) {
      alert("Please enter a valid amount");
      return;
    }
    setShowPaymentModal(true);
  };

  const handleShare = () => {
    alert("Share button clicked!");
  };

  const handlePaymentStatus = (status, message) => {
    // Update the payment status message
    setPaymentMessage({ text: message, type: status });
  };

  if (!campaign) {
    return (
      <div className="dashboard-container">
        <Navbar />
        <div className="no-campaign-selected">
          <p>No campaign selected.</p>
        </div>
        <Footer />
      </div>
    );
  }

  const {
    campaignId,
    projectTitle,
    businessDescription,
    imageUrl,
    raisedAmount = 0,
    fundingGoal = 0,
    volunteerName = "Volunteer",
    followers = [],
    funders = [],
    comments = [],
  } = campaign;

  const progress = fundingGoal > 0 ? (raisedAmount / fundingGoal) * 100 : 0;

  return (
    <div className="dashboard-container">
      <Navbar />

      {paymentMessage && <Message text={paymentMessage.text} type={paymentMessage.type} />} {/* Display message here */}

      <div className="campaign-details">
        <div className="campaign-header">
          <div className="campaign-image2">
            <img
              src={`https://localhost:7182${campaign.businessImage}`} // Ensure the full URL is formed
              alt={campaign.businessName}
              className="campaign-image"
              onError={(e) => e.target.src = "https://via.placeholder.com/300x200"} // Fallback image in case of error
            />
          </div>
          <div className="campaign-text">
            <h2>{projectTitle}</h2>
            <p>{businessDescription}</p>
          </div>
        </div>

        <div className="progress-info">
          <div className="progress-bar-wrapper">
            <div className="progress-bar" style={{ width: `${progress}%` }}></div>
          </div>
          <p>{progress.toFixed(2)}% Complete</p>
          <p>MWK {raisedAmount.toLocaleString()} Raised of MWK {fundingGoal.toLocaleString()} Goal</p>
        </div>

        <div className="campaign-actions">
          <input
            type="number"
            placeholder="Enter amount (e.g., 10.00)"
            value={amount}
            onChange={(e) => setAmount(e.target.value)}
            className="amount-input"
          />
          <button onClick={handleDonate} className="donate-button">Donate Now</button>
          <button onClick={handleShare} className="share-button">Share Campaign</button>
        </div>

        {showPaymentModal && (
          <PaymentModal
            amount={parseFloat(amount)}
            campaignId={campaignId}
            onClose={() => setShowPaymentModal(false)}
            onPaymentStatus={handlePaymentStatus} // Pass payment status handler to PaymentModal
          />
        )}

        <div className="campaign-section">
          <h3>Volunteer</h3>
          <p>{volunteerName}</p>
        </div>

        <div className="campaign-section">
          <h3>Followers ({followers.length})</h3>
          <div className="followers-list">
            {followers.map((follower, index) => (
              <div key={index} className="follower-item">
                <img src={follower.imageUrl || "https://via.placeholder.com/40"} alt="Follower" />
                <div>
                  <p>{follower.name}</p>
                  <p>{new Date(follower.dateFollowed).toLocaleString()}</p>
                </div>
              </div>
            ))}
          </div>
        </div>

        <div className="campaign-section">
          <h3>Funders ({funders.length})</h3>
          <div className="funders-list">
            {funders.map((funder, index) => (
              <div key={index} className="funder-item">
                <img src={funder.imageUrl || "https://via.placeholder.com/40"} alt="Funder" />
                <div>
                  <p>{funder.name}</p>
                  <p>MWK {funder.amount?.toLocaleString()} {funder.date && new Date(funder.date).toLocaleDateString()}</p>
                </div>
              </div>
            ))}
          </div>
        </div>

        <div className="campaign-section">
          <h3>Comments</h3>
          <div className="comments-list">
            {comments.map((comment, index) => (
              <div key={index} className="comment-item">
                <p><strong>{comment.author}</strong> said:</p>
                <p>{comment.text}</p>
              </div>
            ))}
          </div>
        </div>
      </div>

      <Footer />
    </div>
  );
};

export default Campaign;
