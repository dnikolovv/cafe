import * as apiClient from "./apiClient";
import * as webSocketClient from "./webSocketClient";
import { toast } from "react-toastify";

const baseUrl = apiClient.BASE_URL + "/table";

export function addTable(table) {
  return apiClient.post(baseUrl, table);
}

export function loadTables() {
  return apiClient.get(baseUrl);
}

export function subscribeToTableActions() {
  webSocketClient.subscribeTo("tableActions", "WaiterCalled", e => {
    debugger;
    toast.success(`You've been called at table ${e.tableNumber}.`);
  });

  webSocketClient.subscribeTo("tableActions", "BillRequested", e => {
    debugger;
    toast.success(`Table ${e.tableNumber} has requested their bill.`);
  });
}

export function callWaiter(tableNumber) {
  const url = baseUrl + "/" + tableNumber + "/callWaiter";
  return apiClient.post(url);
}

export function requestBill(tableNumber) {
  const url = baseUrl + "/" + tableNumber + "/requestBill";
  return apiClient.post(url);
}
