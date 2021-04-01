import { h } from "preact";
import style from "./style.css";

const Btn = ({ label, visible, action }) => {
  return (
    <button
      style={visible ? "display:inline" : "display: none"}
      onClick={action}
    >
      {label}
    </button>
  );
};

export default Btn;
