import { h } from "preact";
import style from "./style.css";

export const MyTabHeader = ({ children }) => {
  return <div class={style.tab}>{children}</div>;
};
