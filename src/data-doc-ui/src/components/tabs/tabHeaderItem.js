import { h } from "preact";
import style from "./style.css";

export const TabHeaderItem = ({ children, active, onSelect = () => {} }) => {
  return (
    <button class={active ? style.active : {}} onClick={handleClick}>
      {children}
    </button>
  );

  function handleClick() {
    onSelect();
  }
};
