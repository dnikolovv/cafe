import * as apiClient from "./apiClient";
import * as webSocketClient from "./webSocketClient";
import { toast } from "react-toastify";
import { joinUrlWithRoute } from "../utils/urlUtils";

const baseUrl = joinUrlWithRoute(apiClient.BASE_URL, "/table");

export function addTable(table) {
  return apiClient.post(baseUrl, table);
}

export function loadTables() {
  return apiClient.get(baseUrl);
}

export function subscribeToTableActions() {
  return webSocketClient.subscribeTo("tableActions", [
    {
      eventName: "WaiterCalled",
      callback: e => {
        toast.success(`You've been called at table ${e.tableNumber}.`);
      }
    },
    {
      eventName: "BillRequested",
      callback: e => {
        toast.success(`Table ${e.tableNumber} has requested their bill.`);
      }
    }
  ]);
}

export function callWaiter(tableNumber) {
  const url = joinUrlWithRoute(baseUrl, tableNumber + "/callWaiter");
  return apiClient.post(url);
}

export function requestBill(tableNumber) {
  const url = joinUrlWithRoute(baseUrl, tableNumber + "/requestBill");
  return apiClient.post(url);
}
