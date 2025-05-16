// src/components/Message.jsx
import React from "react";
import "./../styles/common.css";

const Message = ({ text, type = "success" }) => {
  return (
    <div className={`message ${type}`}>
      {text}
    </div>
  );
};

export default Message;
