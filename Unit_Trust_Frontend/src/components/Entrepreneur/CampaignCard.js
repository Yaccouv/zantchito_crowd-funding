import React, { useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import axios from 'axios';
import Navbar from "../Navbar";
import Footer from "../Footer";
import "../../styles/common.css";

const CampaignCard = () => {
  const [campaigns, setCampaigns] = useState([]);
  const navigate = useNavigate();

  const fetchCampaigns = async () => {
    try {
      const response = await axios.get(`https://localhost:7182/api/Campaign/getActiveCampaigns`);
      setCampaigns(response.data);
    } catch (error) {
      console.error("Error fetching campaigns", error);
    }
  };

  useEffect(() => {
    fetchCampaigns();
  }, []);

  const handleCardClick = (campaign) => {
    navigate('/campaign', { state: { campaign } });
  };

  const calculateDaysLeft = (endDate) => {
    const deadline = new Date(endDate); // Using the campaign's endDate
    const today = new Date();
    const diffTime = deadline - today;
    const daysLeft = Math.ceil(diffTime / (1000 * 60 * 60 * 24));
    return daysLeft >= 0 ? daysLeft : 0; // Return 0 if the campaign has ended
  };

  return (
    <div className="dashboard-container">
      <Navbar />
      <div className="active-campaigns-container">
        <h2>Active Projects</h2>
        <div className="campaign-cards-container">
          {campaigns.length === 0 ? (
            <p>No active campaigns at the moment.</p>
          ) : (
            campaigns.map((campaign) => {
              const raisedAmount = campaign.raisedAmount || 0;
              const fundingGoal = campaign.fundingGoal || 0;
              const progress = fundingGoal > 0 ? (raisedAmount / fundingGoal) * 100 : 0;
              const daysLeft = calculateDaysLeft(campaign.endDate);

              return (
                <div key={campaign.campaignId} className="campaign-card" onClick={() => handleCardClick(campaign)}>
                  <img
                    src={`https://localhost:7182${campaign.businessImage}`}
                    alt={campaign.businessName}
                    className="campaign-image"
                    onError={(e) => e.target.src = "https://via.placeholder.com/300x200"}
                  />
                  <div className="campaign-info">
                    <h3 className="campaign-title">{campaign.projectTitle}</h3>
                    <p className="campaign-business-name">{campaign.businessName}</p>
                    <div className="progress-container">
                      <div className="progress-bar" style={{ width: `${progress}%` }}></div>
                    </div>
                    <div className="funding-info">
                      <p className="funds-raised">
                        {daysLeft > 0 ? `${daysLeft} day(s) remaining` : "Campaign ended"}
                      </p>
                      <p className="funds-raised">MWK{raisedAmount.toLocaleString()} Raised</p>
                      <p className="funding-goal">MWK{fundingGoal.toLocaleString()} Goal</p>
                      <p className="progress-percentage">{progress.toFixed(2)}% Complete</p>
                    </div>
                  </div>
                </div>
              );
            })
          )}
        </div>
      </div>
      <Footer />
    </div>
  );
};

export default CampaignCard;
