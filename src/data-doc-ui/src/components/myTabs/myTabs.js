import { h } from "preact";
import { MyTabHeader } from "./myTabsHeader";
import { MyTabContent } from "./myTabContent";
import { MyTabHeaderItem } from "./myTabHeaderItem";
import style from "./style.css";

export const MyTabs = ({
  children,
  activeTab = 0,
  renderInactive,
  onChangeTab,
}) => {
  const { header, body } = tabsToRender(
    children,
    activeTab,
    (tabId) => () => onChangeTab(tabId),
    renderInactive
  );
  console.log({ header, body });
  return (
    <div>
      <MyTabHeader>{header}</MyTabHeader>
      <MyTabContent>{body}</MyTabContent>
    </div>
  );
};

function tabsToRender(tabs, activeTab, getHandleChangeTab, lazyLoad) {
  const initialValue = {
    header: [],
    body: [],
  };

  return tabs.reduce((acc, tab, idx) => {
    const isActive = activeTab === idx;
    const renderContent = isActive || lazyLoad;
    console.log(tab);
    const { title } = tab.props;

    const HeaderItem = (
      <MyTabHeaderItem
        key={idx}
        active={isActive}
        onSelect={getHandleChangeTab(idx)}
      >
        {title}
      </MyTabHeaderItem>
    );

    const ContentItem = renderContent ? tab : null;

    tab.props.active = isActive;
    tab.key = idx;

    return {
      header: [...acc.header, HeaderItem],
      body: [...acc.body, ContentItem],
    };
  }, initialValue);
}
