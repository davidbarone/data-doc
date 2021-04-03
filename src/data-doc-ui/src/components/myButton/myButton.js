import { h } from "preact";
import style from "./style.css";

const MyButton = ({ label, visible, action }) => {
  return (
    <button
      style={visible ? "display:inline" : "display: none"}
      onClick={action}
    >
      {label}
    </button>
  );
};

export default MyButton;
