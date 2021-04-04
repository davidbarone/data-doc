import { h } from "preact";
import style from "./style.css";
import { useState, useEffect, useRef } from "preact/hooks";

const MyModal = ({ children, state, onClose }) => {
  const [modalClasses, setModalClasses] = useState([]);
  const [contentClasses, setContentClasses] = useState([]);

  const contentDiv = useRef(null);
  const modalDiv = useRef(null);

  useEffect(() => {
    setModalClasses(
      state[0] ? [style.myModal].join(" ") : [style.myModal].join(" ")
    );

    setContentClasses(
      state[0]
        ? [style.myModalContent].join(" ")
        : [style.myModalContent].join(" ")
    );

    if (state[0]) {
      contentDiv.current.classList.add(style.slideIn);
      contentDiv.current.classList.remove(style.slideOut);
      modalDiv.current.classList.add(style.fadeIn);
      modalDiv.current.classList.remove(style.fadeOut);
    } else {
      contentDiv.current.classList.remove(style.slideIn);
      contentDiv.current.classList.add(style.slideOut);
      if (modalDiv.current.classList.contains(style.fadeIn)) {
        modalDiv.current.classList.add(style.fadeOut);
        modalDiv.current.classList.remove(style.fadeIn);
      }
    }
  }, [state]);

  const hideModal = () => {
    state[1](false);
    if (onClose) {
      onClose();
    }
  };

  const closeStyle = {
    position: "absolute",
    top: "10px",
    right: "10px",
  };

  return (
    <div class={modalClasses} ref={modalDiv}>
      <div class={contentClasses} ref={contentDiv}>
        {children}
        <button
          style={closeStyle}
          onClick={() => {
            hideModal();
          }}
        >
          X
        </button>
      </div>
    </div>
  );
};

export default MyModal;
