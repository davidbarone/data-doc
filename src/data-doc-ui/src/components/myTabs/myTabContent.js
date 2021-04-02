import { h } from "preact";
import style from "./style.css";

export const MyTabContent = ({ children }) => {
  return <div class={style.tabcontent}>{children}</div>;
};
