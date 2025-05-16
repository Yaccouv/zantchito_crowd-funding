import React, { useEffect, useState } from "react";
import axios from "axios";
import { useNavigate } from "react-router-dom";
import Navbar from "../Navbar";
import Footer from "../Footer";
import Message from "../Message"; // Import Message component
import "../../styles/Staff/ApplicantsTable.css";

const CampaignList = () => {
  const [campaigns, setCampaigns] = useState([]);
  const [error, setError] = useState(null);
  const [selectedRow, setSelectedRow] = useState(null);
  const [isModalOpen, setIsModalOpen] = useState(false);
  const [pageMessage, setPageMessage] = useState(""); // Message for the page

  const navigate = useNavigate();

  const fetchCampaigns = async () => {
    try {
      const response = await axios.get(
        "https://localhost:7182/api/Campaign/getCampaigns"
      );
      setCampaigns(response.data);
    } catch (error) {
      setError("Error fetching campaigns.");
    }
  };

  const handleRowClick = async (campaign) => {
    try {
      const response = await axios.get(
        `https://localhost:7182/api/Campaign/getUserById/${campaign.userId}`
      );
      const user = response.data;

      const combinedData = {
        campaignId: campaign.campaignId,
        Firstname: user.firstname,
        Lastname: user.lastname,
        Email: user.email,
        BusinessName: campaign.businessName,
        BusinessCategory: campaign.businessCategory,
        ProjectTitle: campaign.projectTitle,
        FundingGoal: campaign.fundingGoal,
        BusinessDescription: campaign.businessDescription,
        BusinessImage: campaign.businessImage,  // This is the image
        IDImage: campaign.idImage,  // This is the PDF
      };

      setSelectedRow(combinedData);
      setIsModalOpen(true);
    } catch (error) {
      console.error("Error fetching user data", error);
    }
  };

  const handleAccept = async () => {
    if (!selectedRow || !selectedRow.campaignId) {
      console.error("Campaign ID is missing!");
      return;
    }

    try {
      await axios.put(
        `https://localhost:7182/api/Campaign/updateStatus/${selectedRow.campaignId}`,
        JSON.stringify("1"),
        {
          headers: {
            "Content-Type": "application/json",
          },
        }
      );
      console.log("Campaign Accepted:", selectedRow.businessName);
      setPageMessage(`🎉 Campaign for the Entrepreneur has been Accepted!`); // Set page message
      closeModal();
      fetchCampaigns();
    } catch (error) {
      console.error("Error in handleAccept:", error);
    }
  };

  const handleDeny = async () => {
    if (!selectedRow || !selectedRow.campaignId) {
      console.error("Campaign ID is missing!");
      return;
    }

    try {
      await axios.put(
        `https://localhost:7182/api/Campaign/updateStatus/${selectedRow.campaignId}`,
        JSON.stringify("2"),
        {
          headers: {
            "Content-Type": "application/json",
          },
        }
      );
      console.log("Campaign Denied:", selectedRow.businessName);
      setPageMessage(`❌ Campaign for the Entrepreneur has been Denied!`); // Set page message
      closeModal();
      fetchCampaigns();
    } catch (error) {
      console.error("Error in handleDeny:", error);
    }
  };

  const closeModal = () => {
    setIsModalOpen(false);
    setTimeout(() => setPageMessage(""), 5000); // Clear message after 5 seconds
  };

  useEffect(() => {
    fetchCampaigns();
  }, []);

  if (error) return <div>{error}</div>;

  return (
    <div className="dashboard-container">
      <Navbar />
      <div className="main-content">
        <h2>Campaign List</h2>

        {/* Display the message using the Message component */}
        {pageMessage && <Message text={pageMessage} type="success" />}

        <table className="applicant-table">
          <thead>
            <tr>
              <th>Business Name</th>
              <th>Action</th>
            </tr>
          </thead>
          <tbody>
            {campaigns.length === 0 ? (
              <tr>
                <td colSpan="2">No campaigns available.</td>
              </tr>
            ) : (
              campaigns.map((campaign, index) => (
                <tr
                  key={index}
                  onClick={() => handleRowClick(campaign)}
                  style={{ cursor: "pointer" }}
                >
                  <td>{campaign.businessName}</td>
                  <td>View</td>
                </tr>
              ))
            )}
          </tbody>
        </table>

        {isModalOpen && selectedRow && (
          <div className="modal-overlay">
            <div className="modal">
              <h2>Campaign Data for: {selectedRow.businessName}</h2>
              {Object.entries(selectedRow).map(([key, value]) => (
                key !== "campaignId" && key !== "BusinessImage" && key !== "IDImage" && (
                  <div className="form-group" key={key}>
                    <label>{capitalizeFirstLetter(key)}</label>
                    <p>{value}</p>
                  </div>
                )
              ))}

              {selectedRow.BusinessImage && (
                <div className="form-group">
                  <label>Business Image</label>
                  <img
                    src={`https://localhost:7182${selectedRow.BusinessImage}`}
                    alt="Business"
                    className="modal-image"
                  />
                </div>
              )}

              {selectedRow.IDImage && (
                <div className="form-group">
                  <label>ID Image (PDF)</label>
                  <embed
                    src={`https://localhost:7182${selectedRow.IDImage}`}
                    type="application/pdf"
                    width="100%"
                    height="600px"
                  />
                </div>
              )}

              <div className="button-group">
                <button onClick={handleAccept} className="accept-button">
                  Accept
                </button>
                <button onClick={handleDeny} className="deny-button">
                  Deny
                </button>
              </div>
            </div>
          </div>
        )}

        <Footer />
      </div>
    </div>
  );
};

const capitalizeFirstLetter = (str) => {
  return str.charAt(0).toUpperCase() + str.slice(1);
};

export default CampaignList;
