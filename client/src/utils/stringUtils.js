export function trimStart(character, string) {
  var startIndex = 0;

  while (string[startIndex] === character) {
    startIndex++;
  }

  return string.substr(startIndex);
}

export function trimEnd(character, string) {
  // Because why the hell not
  return reverse(trimStart(character, reverse(string)));
}

function reverse(string) {
  return string
    .split("")
    .reverse()
    .join("");
}
