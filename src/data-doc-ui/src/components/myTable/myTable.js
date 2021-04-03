import { h } from "preact";
import style from "./style.css";

const MyTable = ({ data, mapping }) => {
  return (
    <div>
      <table class={style.myTable}>
        <thead>
          <tr>
            {Object.keys(mapping).map((k) => (
              <th>{k}</th>
            ))}
          </tr>
        </thead>
        <tbody>
          {data.map((row) => (
            <tr>
              {Object.keys(mapping).map((k) => (
                <td>{mapping[k](row)}</td>
              ))}
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
};

export default MyTable;
