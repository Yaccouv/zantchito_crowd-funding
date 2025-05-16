import React from "react";
import '../../styles/Investor/home.css';  // Import the Home component style

const Home = () => {
    console.log("Home component rendered");
    return (
        <div className="home-container">
            <h2>Zantchito Skils for Jobs</h2>
            <div>
                <button>
                    <a href="/signin">SIGN IN</a>
                </button>
            </div><br />
            <div>
                <button>
                    <a href="/signup">SIGN UP</a>
                </button>
            </div>
        </div>
    );
};

export default Home;
