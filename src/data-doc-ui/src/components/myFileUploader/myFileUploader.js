import { h } from "preact";
import style from "./style.css";
import { useRef } from "preact/hooks";

const MyFileUploader = ({ name, label, action, visible = true }) => {
  const inputNode = useRef(null);

  const file = () => inputNode.current.files[0];

  const clickHandler = (event) => {
    action(event, file());
    event.preventDefault();
    return false;
  };

  return (
    <div
      class={style.field}
      style={visible ? "display:inline" : "display: none"}
    >
      <input id="upload" type="file" name={name} ref={inputNode} />
      <button class={style.myButton} onClick={clickHandler}>
        {label}
      </button>
    </div>
  );
};

export default MyFileUploader;
