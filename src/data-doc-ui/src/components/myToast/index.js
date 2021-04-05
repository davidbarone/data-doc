/* 
https://github.com/ashr81/react-tiny-toast
https://www.npmjs.com/package/react-tiny-toast
*/

import toast from "./toast";
import MyToastContainer from "./MyToastContainer";
export const POSITIONS = {
  TOP_CENTER: "top-center",
  TOP_LEFT: "top-left",
  TOP_RIGHT: "top-right",
  BOTTOM_LEFT: "bottom-left",
  BOTTOM_RIGHT: "bottom-right",
  BOTTOM_CENTER: "bottom-center",
};
export const VARIANTS = {
  SUCCESS: "success",
  DANGER: "danger",
  WARNING: "warning",
  DEFAULT: "default",
};
export const ACTIONS = {
  ADD: "ADD",
  REMOVE: "REMOVE",
};

export { toast, MyToastContainer };
