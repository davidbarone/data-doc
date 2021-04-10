import { h } from "preact";
import style from "./style.css";
import { useRef } from "preact/hooks";

const MyDropdown = ({
  name,
  label,
  values,
  texts, // can be omitted, in which case, values are used as text
  multiple,
  size,
  selectedValue,
  target,
  setTarget,
  disabled,
  onInputHook,
  nullText,
}) => {
  const selectNode = useRef(null);

  const onChange = (e) => {
    let val = e.target.value;

    // HTML <option> does not support null values. Must convert '' to null
    if (val === "") {
      val = null;
    }

    // If multiple select, we must get the value a slightly longer way.
    if (multiple) {
      val = Array.from(selectNode.current.options)
        .filter((o) => o.selected)
        .map((o) => o.value);
    }
    setTarget({ ...target, [e.target.name]: val });

    if (onInputHook) {
      onInputHook(e);
    }
  };

  return (
    <div class={style.field}>
      <label>{label}:</label>
      <select
        ref={selectNode}
        disabled={disabled}
        multiple={multiple}
        size={size}
        name={name}
        onChange={onChange}
      >
        {(nullText !== null ? [null, ...values] : values).map((v) => (
          <option
            label={
              v === null
                ? nullText
                : typeof texts != "undefined" &&
                  texts !== null &&
                  values.filter((v) => v !== null).indexOf(v) < texts.length
                ? texts[values.filter((v) => v !== null).indexOf(v)]
                : v
            }
            value={v}
            selected={
              Array.isArray(selectedValue)
                ? selectedValue.indexOf(v) >= 0
                : selectedValue === v
            }
          />
        ))}
      </select>
    </div>
  );
};

export default MyDropdown;
