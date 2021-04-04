import { h } from "preact";
import style from "./style.css";

const MyButton = ({ label, visible = true, action }) => {
  return (
    <button
      class={style.myButton}
      style={visible ? "display:inline" : "display: none"}
      onClick={action}
    >
      {label}
    </button>
  );
};

export default MyButton;
