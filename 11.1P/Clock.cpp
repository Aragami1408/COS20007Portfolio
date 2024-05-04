
#include <iostream>
#include <string>
#include <chrono>
#include <thread>

class Counter {
  private:
    int _count;
    std::string _name;

  public:
    Counter(std::string name) : _name(name), _count(0) {}

    void Increment() {
      _count++;
    }

    void Reset() {
      _count = 0;
    }

    std::string GetName() const {
      return _name;
    }

    void SetName(std::string name) {
      _name = name;
    }

    int GetTicks() const {
      return _count;
    }

    void SetTicks(int value) {
      _count = value;
    }
};

class Clock {
  private:
    Counter _hours;
    Counter _minutes;
    Counter _seconds;

  public:
    Clock() : _hours("Hours"), _minutes("Minutes"), _seconds("Seconds") {
      _hours.Reset();
      _minutes.Reset();
      _seconds.Reset();
    }

    void Tick() {
      _seconds.Increment();
      if (_seconds.GetTicks() == 60) {
        _minutes.Increment();
        _seconds.Reset();
        if (_minutes.GetTicks() == 60) {
          _hours.Increment();
          _minutes.Reset();
          if (_hours.GetTicks() == 24) {
            Reset();
          }
        }
      }
    }

    void Reset() {
      _seconds.Reset();
      _minutes.Reset();
      _hours.Reset();
    }

    std::string CurrentTime() {
      return std::to_string(_hours.GetTicks()) + ":" + std::to_string(_minutes.GetTicks()) + ":" + std::to_string(_seconds.GetTicks());
    }

    Counter& GetHours() {
      return _hours;
    }

    Counter& GetMinutes() {
      return _minutes;
    }

    Counter& GetSeconds() {
      return _seconds;
    }
};


int main() {
  Clock clock;

  for (int i = 0; i < 10; i++) {
    std::cout << "Current Time: " << clock.CurrentTime() << std::endl;
    clock.Tick();
    std::this_thread::sleep_for(std::chrono::milliseconds(1000));
  }

  return 0;
}
