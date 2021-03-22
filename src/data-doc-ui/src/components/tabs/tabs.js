import { h } from "preact";
import { TabHeader } from "./tabsHeader";
import { TabContent } from "./tabContent";
import { TabHeaderItem } from "./tabHeaderItem";
import style from "./style.css";

export const Tabs = ({
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
      <TabHeader>{header}</TabHeader>
      <TabContent>{body}</TabContent>
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
      <TabHeaderItem
        key={idx}
        active={isActive}
        onSelect={getHandleChangeTab(idx)}
      >
        {title}
      </TabHeaderItem>
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
