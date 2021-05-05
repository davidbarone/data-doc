import { h } from "preact";
import style from "./style.css";
import { useRef, useState } from "preact/hooks";

const MyFileUploader = ({ name, label, action, visible = true }) => {
  //const inputNode = useRef(null);
  const [uploadState, setUploadState] = useState("INITIAL"); // INITIAL | SAVING | SUCCESS | FAILURE

  //const file = () => inputNode.current.files[0];

  const uploadFile = (e) => {
    setUploadState("SAVING");
    const file = e.target.files[0];
    setTimeout(() => {
      action(e, file);
      setUploadState("INITIAL");
      e.preventDefault();
    }, 0);
  };

  return (
    <label class={style.dropbox}>
      <input
        type="file"
        //ref={inputNode}
        name={name}
        disabled={uploadState === "SAVING"}
        onChange={uploadFile}
        accept="*"
        class={style.inputFile}
      />
      <p style={{ display: uploadState === "INITIAL" ? "inline" : "none" }}>
        {label}
      </p>
      <p style={{ display: uploadState === "SAVING" ? "inline" : "none" }}>
        Uploading file...
      </p>
    </label>
  );
};

export default MyFileUploader;
