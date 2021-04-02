import { h } from "preact";
import style from "./style.css";

const Field = ({ name, type, label, target, disabled, rows, onInputHook }) => {
  const onInput = (e) => {
    const val =
      e.target.type === "checkbox" ? e.target.checked : e.target.value;
    const target = { ...target, [e.target.name]: val };

    if (onInputHook) {
      onInputHook(e);
    }
  };

  const input = () => {
    if (type === "input" && rows > 0) {
      return (
        <textarea
          rows={rows}
          disabled={disabled}
          class={disabled ? style.readonly : style.writeable}
          name={name}
          value={target[name]}
          onInput={onInput}
        />
      );
    } else if (type === "checkbox") {
      return (
        <input
          disabled={disabled}
          class={disabled ? style.readonly : style.writeable}
          type="checkbox"
          name={name}
          checked={target[name]}
          onClick={onInput}
        />
      );
    }
    return (
      <input
        disabled={disabled}
        class={disabled ? style.readonly : style.writeable}
        type={type}
        name={name}
        value={target[name]}
        onInput={onInput}
      />
    );
  };

  return (
    <div class={style.field}>
      <label>{label}:</label>
      {input()}
    </div>
  );
};

export default Field;
