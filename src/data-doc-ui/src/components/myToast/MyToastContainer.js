import { h } from "preact";
import { useEffect, useRef, useReducer } from "preact/hooks";
import { toastManager } from "./toast";
import style from "./style.css";

const ADD = "ADD";
const REMOVE = "REMOVE";

const reducer = (state, action) => {
  const { type, data } = action;
  if (type === ADD) {
    if (
      state.filter((i) => i.uniqueCode && i.uniqueCode === data.uniqueCode)
        .length
    ) {
      return state;
    }
    return [...state, data];
  } else if (type === REMOVE) {
    return state.filter((i) => i.id !== data.id);
  }
  return state;
};

const MyToastContainer = () => {
  const toastRootElementId = "myToast-main-container";
  const [data, dispatch] = useReducer(reducer, []);
  const toastRef = useRef(null);

  const callback = (actionType, content, options) => {
    if (actionType === ADD) {
      dispatch({
        type: ADD,
        data: { content, ...options, key: `${options.id}` },
      });
    }
    if (options.pause && actionType === REMOVE) {
      dispatch({ type: REMOVE, data: { id: options.id } });
    } else if (!options.pause) {
      window.setTimeout(() => {
        dispatch({ type: REMOVE, data: { id: options.id } });
      }, options.timeout);
    }
  };

  useEffect(() => {
    toastManager.subscribe(callback);
  }, []);

  useEffect(() => {
    const node = document.createElement("div");
    node.setAttribute("id", toastRootElementId);
    node.classList.add(style[toastRootElementId]);
    document.body.appendChild(node);
    toastRef.current = node;
    return () => document.body.removeChild(node);
  }, []);

  const positionMaintainer = () => {
    const mapper = {};
    data.map(({ position, ...rest }) => {
      if (position) {
        if (!mapper[position]) mapper[position] = [];
        mapper[position].push(rest);
      }
    });
    console.log(mapper);
    return mapper;
  };

  const markup = () => {
    const mapper = positionMaintainer();
    return Object.keys(mapper).map((position, index) => {
      const content = mapper[position].map(
        ({ key, content, variant, className }) => {
          let animationCssClass = "myToast-item-animation-top";
          if (position.indexOf("bottom"))
            animationCssClass = "myToast-item-animation-bottom";
          return (
            <div
              key={key}
              class={[
                style["myToast-item"],
                style[`myToast-item-${variant}`],
                style[`${animationCssClass}`],
                style[`${className ? className : ""}`],
              ].join(" ")}
            >
              {content}
            </div>
          );
        }
      );
      return (
        <div
          key={index}
          class={[style["myToast-container"], style[`${position}`]].join(" ")}
        >
          {content}
        </div>
      );
    });
  };

  if (!toastRef.current) return null;
  return (
    <div class={style["myToast-main-container"]} ref={toastRef}>
      {markup()}
    </div>
  );
};

export default MyToastContainer;
