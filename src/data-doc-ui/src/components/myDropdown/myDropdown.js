import { h } from "preact";
import style from "./style.css";
import { useRef } from "preact/hooks";

const MyDropdown = ({
  name,
  label,
  values,
  multiple,
  size,
  selectedValue,
  target,
  setTarget,
  disabled,
  onInputHook,
}) => {
  const selectNode = useRef(null);

  const onChange = (e) => {
    let val = e.target.value;
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
        {values.map((v) => (
          <option
            label={v}
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
