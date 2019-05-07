import React, { useState, useEffect } from "react";
import PropTypes from "prop-types";

const FiniteSelctableMenu = ({
  itemPairs,
  onSelectedPairsChanged,
  ...props
}) => {
  const [selectedPairs, setSelectedPairs] = useState([]);

  useEffect(() => {
    if (props.selectedPairs) {
      setSelectedPairs(props.selectedPairs);
    }
  }, [props.selectedPairs]);

  const addPair = pair => {
    const existing = selectedPairs.find(
      p => p.item.number === pair.item.number
    );

    let newSelectedPairs = [...selectedPairs];

    if (existing) {
      if (
        existing.count <
        itemPairs.find(p => p.item.number === existing.item.number).count
      ) {
        newSelectedPairs = [
          ...selectedPairs.filter(p => p.item.number !== existing.item.number),
          { ...existing, count: existing.count + 1 }
        ];
      }
    } else {
      newSelectedPairs = [...selectedPairs, { count: 1, item: pair.item }];
    }

    setSelectedPairs(newSelectedPairs);

    if (onSelectedPairsChanged) {
      onSelectedPairsChanged(newSelectedPairs);
    }
  };

  const removePair = pair => {
    const existing = selectedPairs.find(
      p => p.item.number === pair.item.number
    );

    if (existing && existing.count > 0) {
      const nextCount = existing.count - 1;

      const newSelectedPairs =
        nextCount > 0
          ? [
              ...selectedPairs.filter(
                p => p.item.number !== existing.item.number
              ),
              { ...existing, count: existing.count - 1 }
            ]
          : [
              ...selectedPairs.filter(
                p => p.item.number !== existing.item.number
              )
            ];

      setSelectedPairs(newSelectedPairs);

      if (onSelectedPairsChanged) {
        onSelectedPairsChanged(newSelectedPairs);
      }
    }
  };

  return (
    <>
      {itemPairs.length > 0 ? (
        <ul className="list-group">
          {itemPairs.map(pair => {
            return (
              <li key={pair.item.number} className="list-group-item">
                {`${pair.count} x ${pair.item.description}`}
                <button
                  onClick={() => removePair(pair)}
                  className="btn btn-danger btn-sm float-right"
                  disabled={
                    !selectedPairs.some(
                      si => si.item.number === pair.item.number
                    )
                  }
                >
                  <i className="fa fa-minus" />
                </button>
                <button
                  onClick={() => addPair(pair)}
                  className="btn btn-success btn-sm float-right mr-1"
                >
                  <i className="fa fa-plus" />
                </button>
              </li>
            );
          })}
          <li className="list-group-item list-group-item-dark">
            {selectedPairs.length > 0 ? (
              <>
                {selectedPairs
                  .sort((a, b) => a.item.number - b.item.number)
                  .map(item => `${item.count} x ${item.item.description}`)
                  .join(", ")}
              </>
            ) : (
              <>Nothing selected</>
            )}
          </li>
        </ul>
      ) : (
        <ul className="list-group">
          <li className="list-group-item list-group-item-dark">
            No items to display.
          </li>
        </ul>
      )}
    </>
  );
};

FiniteSelctableMenu.propTypes = {
  selectedPairs: PropTypes.array.isRequired,
  onSelectedPairsChanged: PropTypes.func.isRequired,
  itemPairs: PropTypes.array.isRequired
};

export default FiniteSelctableMenu;
