import { h } from "preact";
import style from "./style.css";

const MyDropdown = ({
  name,
  label,
  values,
  selectedValue,
  target,
  disabled,
  onInputHook,
}) => {
  const onChange = (e) => {
    const val = e.target.value;
    target = { ...target, [e.target.name]: val };

    if (onInputHook) {
      onInputHook(e);
    }
  };

  return (
    <div class={style.field}>
      <label>{label}:</label>
      <select
        disabled={disabled}
        name={name}
        onChange={onChange}
        value={selectedValue}
      >
        {values.map((v) => (
          <option label={v} value={v} />
        ))}
      </select>
    </div>
  );
};

export default MyDropdown;
