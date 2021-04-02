import { h } from "preact";

export const MyTab = ({ children, active }) => {
  const cssClass = active ? "active" : "";

  return (
    <div role="tabpanel" class={`tab-pane ${cssClass}`}>
      {children}
    </div>
  );
};
