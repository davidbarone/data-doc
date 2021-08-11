import { h } from "preact";
import style from "./style.css";

const MyButton = ({ label, visible = true, action, title }) => {
  return (
    <button
      class={style.myButton}
      title={title}
      style={visible ? "display:inline" : "display: none"}
      onClick={action}
    >
      {label}
    </button>
  );
};

export default MyButton;
