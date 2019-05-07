import React, { useState } from "react";
import Input from "../common/Input";

const CloseTabForm = ({ owedAmount, onSubmit }) => {
  const [amountPaid, setAmountPaid] = useState(0);

  const handleChange = event => {
    setAmountPaid(event.target.value);
  };

  const handleSubmit = event => {
    event.preventDefault();
    onSubmit(amountPaid);
  };

  return (
    <>
      <h4>Close tab</h4>
      <form onSubmit={handleSubmit}>
        <Input
          isNumber
          label={"Amount Paid (Owed: $" + owedAmount + ")"}
          name="amountPaid"
          onChange={handleChange}
          placeholder="Amount paid..."
          value={amountPaid}
        />
        <input type="submit" className="btn btn-danger" value="Close tab" />
      </form>
    </>
  );
};

export default CloseTabForm;
