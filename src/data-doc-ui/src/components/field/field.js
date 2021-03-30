import { h } from "preact";
import style from "./style.css";

const Field = ({ name, label, target, readOnly }) => {
  const onInput = (e) => {
    const val =
      e.target.type === "checkbox" ? e.target.checked : e.target.value;
    const target = { ...target, [e.target.name]: val };
  };

  return (
    <div class={style.field}>
      <label>{label}:</label>
      <input
        readOnly={readOnly}
        class={readOnly ? style.readonly : style.writeable}
        type="text"
        name={name}
        value={target[name]}
        onInput={onInput}
      />
    </div>
  );
};

export default Field;
